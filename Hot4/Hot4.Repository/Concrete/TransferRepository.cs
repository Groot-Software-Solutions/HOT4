using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.Core.Settings;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hot4.Repository.Concrete
{
    public class TransferRepository : RepositoryBase<Transfer>, ITransferRepository
    {
        private ICommonRepository _commonRepository;
        private ValueSettings _valueSettings { get; }
        public TransferRepository(HotDbContext context, ICommonRepository commonRepository, IOptions<ValueSettings> valueSettings) : base(context)
        {
            _commonRepository = commonRepository;
            _valueSettings = valueSettings.Value;
        }
        public async Task<bool> AddTransfer(Transfer transfer)
        {
            await Create(transfer);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteTransfer(Transfer transfer)
        {
            Delete(transfer);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateTransfer(Transfer transfer)
        {
            Update(transfer);
            await SaveChanges();
            return true;
        }
        public async Task<List<Transfer>> GetTransferByPaymentId(long paymentId)
        {
            return await _context.Transfer.Include(d => d.Channel)
                .Where(d => d.PaymentIdFrom == paymentId || d.PaymentIdTo == paymentId)
                .OrderByDescending(d => d.TransferId).ToListAsync();
            //.Select(d => new TransferModel
            //{
            //    Amount = d.Amount,
            //    ChannelId = d.ChannelId,
            //    ChannelName = d.Channel.Channel,
            //    PaymentIdFrom = d.PaymentIdFrom,
            //    PaymentIdTo = d.PaymentIdTo,
            //    SMSId = d.Smsid,
            //    TransferDate = d.TransferDate,
            //    TransferId = d.TransferId,
            //}).ToListAsync();
        }
        public async Task<Transfer?> GetTransferById(long transferId)
        {
            return await _context.Transfer.Include(d => d.Channel)
    .Where(d => d.TransferId == transferId)
    .FirstOrDefaultAsync();
        }
        public async Task<List<Transfer>> ListTransfer(int pageNo, int pageSize)
        {
            var result = _context.Transfer.Include(d => d.Channel).OrderByDescending(d => d.TransferId);
            return await PaginationFilter.GetPagedData(result, pageNo, pageSize)
                                .Queryable.ToListAsync();

        }
        public async Task<decimal> GetStockTradeBalByAccountId(long accountId)
        {
            var balance = await _context.Payment
                                 .Where(d => d.AccountId == accountId
                                 && d.PaymentTypeId == (int)PaymentMethodType.zBalanceBF
                                 && d.PaymentSourceId == (int)PaymentMethodSource.MCExecutive
                                 && d.Reference == _valueSettings.StockTradePaymentByRef)
                                 .OrderByDescending(d => d.PaymentId)
                                 .Select(d => d.Amount)
                                 .FirstOrDefaultAsync();

            var traded = await _context.Payment
                                .Where(d => d.AccountId == accountId
                                && d.PaymentTypeId == (int)PaymentMethodType.zServiceFees
                                && d.PaymentSourceId == (int)PaymentMethodSource.TradeInUSD)
                                .SumAsync(p => Math.Abs(p.Amount));


            return balance - traded;
        }
        public async Task<StockTradeModel> GetStockTrade(StockTradeSearchModel stockTradeSearch)
        {
            decimal? balance = 0, traded = 0, tradableZWL = 0, ZWLBalance = 0, tradableBalanceZWL = 0, USDBalance = 0;
            decimal paymentAmount = 0;

            balance = await _context.Payment.Where(d => d.AccountId == stockTradeSearch.AccountId
                            && d.PaymentTypeId == (int)PaymentMethodType.zBalanceBF
                            && d.PaymentSourceId == (int)PaymentMethodSource.MCExecutive
                             && d.Reference == _valueSettings.StockTradePaymentByRef)
                            .OrderByDescending(d => d.PaymentId)
                           .Select(d => (decimal?)d.Amount)
                           .FirstOrDefaultAsync();

            traded = await _context.Payment.Where(d => d.AccountId == stockTradeSearch.AccountId
                     && d.PaymentTypeId == (int)PaymentMethodType.zServiceFees
                     && d.PaymentSourceId == (int)PaymentMethodSource.TradeInUSD)
                     .SumAsync(d => (decimal?)d.Amount) ?? 0;

            // Calculate tradable ZWL and balances
            tradableZWL = (balance ?? 0) - traded;
            ZWLBalance = await _commonRepository.GetBalance(stockTradeSearch.AccountId);
            USDBalance = await _commonRepository.GetUSDBalance(stockTradeSearch.AccountId);

            // Calculate tradable balance ZWL and payment amount
            tradableBalanceZWL = ZWLBalance > tradableZWL ? tradableZWL : ZWLBalance;
            paymentAmount = stockTradeSearch.Amount * (stockTradeSearch.Rate == 0 ? _valueSettings.StockTradeRateNullValue : stockTradeSearch.Rate);

            if (paymentAmount <= tradableBalanceZWL)
            {
                if (stockTradeSearch.Amount > 0)
                {

                    var payment = new Payment
                    {
                        AccountId = stockTradeSearch.AccountId,
                        PaymentTypeId = (int)PaymentMethodType.zServiceFees,
                        PaymentSourceId = (int)PaymentMethodSource.TradeInUSD,
                        Amount = -paymentAmount,
                        PaymentDate = DateTime.Now,
                        Reference = $"ZWL Transfer of {stockTradeSearch.Amount}AU @ ROE: {stockTradeSearch.Rate}",
                        LastUser = "HotSystem"
                    };
                    await _context.Payment.AddAsync(payment);
                    await _context.SaveChangesAsync();

                    // Insert reference payment record
                    var referencePayment = new Payment
                    {
                        AccountId = stockTradeSearch.AccountId,
                        PaymentTypeId = (int)PaymentMethodType.USD,
                        PaymentSourceId = (int)PaymentMethodSource.TradeInUSD,
                        Amount = stockTradeSearch.Amount,
                        PaymentDate = DateTime.Now,
                        Reference = $"Ref:{payment.PaymentId}",
                        LastUser = "HotSystem"
                    };
                    await _context.Payment.AddAsync(referencePayment);
                    await _context.SaveChangesAsync();

                    return new StockTradeModel
                    {
                        Result = 1,
                        Message = _valueSettings.StockTradeSuccessResponse,
                        ZWLbalance = await _commonRepository.GetBalance(stockTradeSearch.AccountId),
                        USDBalance = await _commonRepository.GetUSDBalance(stockTradeSearch.AccountId)
                    };
                }
                else
                {
                    // Return error message for invalid amount
                    return new StockTradeModel
                    {
                        Result = -1,
                        Message = _valueSettings.StockTradeInvalidAmountResponse,
                        ZWLbalance = ZWLBalance ?? 0,
                        USDBalance = USDBalance ?? 0
                    };
                }
            }
            else
            {
                // Return error message for insufficient tradable ZWL balance
                return new StockTradeModel
                {
                    Result = -1,
                    Message = _valueSettings.StockTradeInvalidPaymentResponse,
                    ZWLbalance = ZWLBalance ?? 0,
                    USDBalance = USDBalance ?? 0
                };
            }
        }
    }
}
