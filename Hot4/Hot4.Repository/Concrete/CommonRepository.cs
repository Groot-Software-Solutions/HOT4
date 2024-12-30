using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class CommonRepository : ICommonRepository
    {
        public HotDbContext _context;
        public CommonRepository(HotDbContext context)
        {
            _context = context;
        }
        public async Task<float> GetPrePaidStockBalance(int brandId)
        {
            return (float)await (from r in _context.Recharge
                                 where r.BrandId == brandId && r.StateId == (int)SmsState.Success
                                 join rp in _context.RechargePrepaid
                                 on r.RechargeId equals rp.RechargeId
                                 where rp.FinalWallet != 0
                                 orderby r.RechargeId descending
                                 select rp.FinalWallet
                           ).Take(2).DefaultIfEmpty(0).MinAsync();
        }
        public async Task<decimal> GetBalance(long accountId)
        {
            var paymentsSum = await _context.Payment
           .Where(p => !new[] { (int)PaymentMethodType.ZESA, (int)PaymentMethodType.USD, (int)PaymentMethodType.Nyaradzo }.Contains(p.PaymentTypeId)
           && p.AccountId == accountId).SumAsync(p => p.Amount);

            var rechargeSum = await _context.Recharge.Include(d => d.Brand)
                .Where(r => r.Brand.WalletTypeId == (int)WalletTypes.ZWG
                    && new[] { (int)SmsState.Busy, (int)SmsState.Success, (int)SmsState.PendingVerification }.Contains(r.StateId))
                .Join(_context.Access, r => r.AccessId, a => a.AccessId, (r, a) => new { a.AccountId, r.Amount, r.Discount })
                .Where(ra => ra.AccountId == accountId)
                .SumAsync(ra => -(ra.Amount * ((100 - ra.Discount) / 100)));

            return paymentsSum + rechargeSum;

        }

        public async Task<decimal> GetSaleValue(long accountId)
        {
            // Get the average discount
            var discountAvg = await (from d in _context.ProfileDiscount
                                     join a in _context.Account on d.ProfileId equals a.ProfileId
                                     where a.AccountId == accountId
                                     select d.Discount).AverageAsync();

            var balance = await GetBalance(accountId);

            decimal returnValue;
            if (discountAvg != 0)
            {
                returnValue = balance / (1 - (discountAvg / 100));
            }
            else
            {
                returnValue = balance;
            }

            return returnValue;
        }

        public async Task<List<ViewBalanceModel>> GetViewBalanceList(List<long> accountIds)
        {
            var paymentData = await (from p in _context.Payment.Where(d => EF.Constant(accountIds).Contains(d.AccountId))
                                     group p by p.AccountId into g
                                     select new
                                     {
                                         AccountId = g.Key,
                                         amountAirtime = g.Where(x => !new[] { (int)PaymentMethodType.USD, (int)PaymentMethodType.ZESA, (int)PaymentMethodType.Nyaradzo, (int)PaymentMethodType.USDUtility }.Contains(x.PaymentTypeId)).Sum(x => x.Amount),
                                         amountZesa = g.Where(x => new[] { (int)PaymentMethodType.ZESA, (int)PaymentMethodType.Nyaradzo }.Contains(x.PaymentTypeId)).Sum(x => x.Amount),
                                         AmountUSD = g.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USD).Sum(x => x.Amount),
                                         AmountUSDUtility = g.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USDUtility).Sum(x => x.Amount)
                                     }).ToListAsync();

            var rechargeData = await (from r in _context.Recharge
                                      join a in _context.Access on r.AccessId equals a.AccessId
                                      join b in _context.Brand on r.BrandId equals b.BrandId
                                      where new[] { (int)SmsState.Busy, (int)SmsState.Success, (int)SmsState.PendingVerification }.Contains(r.StateId)
                                      group new { r, a, b } by a.AccountId into g
                                      select new
                                      {
                                          AccountId = g.Key,
                                          AmountAirtime = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.ZWG).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100)),
                                          AmountZesa = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.ZESA).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100)),
                                          AmountUSD = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.USD).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100)),
                                          AmountUSDUtility = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.USDUtility).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100))
                                      }).ToListAsync();

            var accountDiscounts = await (from a in _context.Account.Where(d => EF.Constant(accountIds).Contains(d.AccountId))
                                          join pd in _context.ProfileDiscount on a.ProfileId equals pd.ProfileId
                                          group pd by a.AccountId into g
                                          select new
                                          {
                                              AccountId = g.Key,
                                              Discount = g.Average(x => x.Discount)
                                          }).ToListAsync();

            return (from p in paymentData
                    join r in rechargeData on p.AccountId equals r.AccountId into rr
                    from r in rr.DefaultIfEmpty()
                    join d in accountDiscounts on p.AccountId equals d.AccountId
                    select new ViewBalanceModel
                    {
                        AccountId = p.AccountId,
                        Balance = p.amountAirtime + (r == null ? 0 : r.AmountAirtime),
                        ZESABalance = p.amountZesa + (r == null ? 0 : r.AmountZesa),
                        USDBalance = p.AmountUSD + (r == null ? 0 : r.AmountUSD),
                        USDUtilityBalance = p.AmountUSDUtility + (r == null ? 0 : r.AmountUSDUtility),
                        SaleValue = p.amountAirtime / (1 - d.Discount / 100)
                    }).ToList();

        }
        public async Task<List<ViewAccountModel>> GetViewAccountList(List<long> accountIds)
        {
            var bal = await GetViewBalanceList(accountIds);

            var accountList = await (from a in _context.Account.Include(d => d.Profile)
                                     where EF.Constant(accountIds).Contains(a.AccountId)
                                     select new
                                     {
                                         a.AccountId,
                                         a.Profile.ProfileId,
                                         a.Profile.ProfileName,
                                         a.AccountName,
                                         a.NationalId,
                                         a.Email,
                                         a.ReferredBy
                                     })
                         .ToListAsync();

            return (from a in accountList
                    join b in bal on a.AccountId equals b.AccountId into balances
                    from subB in balances.DefaultIfEmpty()
                    select new ViewAccountModel
                    {
                        AccountId = a.AccountId,
                        ProfileId = a.ProfileId,
                        ProfileName = a.ProfileName,
                        AccountName = a.AccountName,
                        NationalId = a.NationalId,
                        EmailId = a.Email,
                        RefferedBy = a.ReferredBy,
                        Balance = subB != null ? subB.Balance : 0,
                        SaleValue = subB != null ? subB.SaleValue : 0,
                        ZESABalance = subB != null ? subB.ZESABalance : 0,
                        USDBalance = subB != null ? subB.USDBalance : 0,
                        USDUtilityBalance = subB != null ? subB.USDUtilityBalance : 0
                    }).OrderByDescending(d => d.Balance).ToList();

        }

    }
}
