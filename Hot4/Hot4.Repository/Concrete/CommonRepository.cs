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
            if (brandId == (int)Brands.ZETDC)
            {
                return (float)await _context.RechargePrepaid.Include(d => d.Recharge)
                    .Where(d => d.Recharge.BrandId == brandId
                    && d.Recharge.StateId == (int)SmsState.Success)
                    .OrderByDescending(d => d.Recharge.RechargeId)
                    .Select(d => d.InitialBalance).LastOrDefaultAsync();
            }
            else
            {
                return (float)await _context.RechargePrepaid.Include(d => d.Recharge)
                    .Where(d => d.Recharge.BrandId == brandId
                    && d.Recharge.StateId == (int)SmsState.Success
                    && d.FinalWallet != 0)
                    .OrderByDescending(d => d.Recharge.RechargeId)
                    .Select(d => d.FinalWallet).Take(2).DefaultIfEmpty(0).MinAsync();
            }
        }

        public async Task<decimal> GetBalance(long accountId)
        {
            var paymentsSum = await _context.Payment
                              .Where(d => !new[]
                              { (int)PaymentMethodType.ZESA,(int)PaymentMethodType.USD,(int)PaymentMethodType.Nyaradzo}
                              .Contains(d.PaymentTypeId)
                               && d.AccountId == accountId).SumAsync(d => d.Amount);

            var rechargeSum = await GetRechargeSum(accountId, (int)WalletTypes.ZWG);

            return paymentsSum + rechargeSum;
        }

        public async Task<decimal> GetUSDBalance(long accountId)
        {
            var paymentsSum = await _context.Payment.Where(p => p.PaymentTypeId == (int)PaymentMethodType.USD
                                     && p.AccountId == accountId).SumAsync(p => p.Amount);

            var rechargeSum = await GetRechargeSum(accountId, (int)WalletTypes.USD);

            return paymentsSum + rechargeSum;
        }
        private async Task<decimal> GetRechargeSum(long accountId, int walletType)
        {
            return await _context.Recharge.Include(d => d.Brand)
                        .Include(d => d.Access)
                        .Where(d => d.Access.AccountId == accountId && d.Brand.WalletTypeId == walletType
                              && new[] { (int)SmsState.Busy, (int)SmsState.Success, (int)SmsState.PendingVerification }
                              .Contains(d.StateId))
                        .SumAsync(d => -(d.Amount * ((100 - d.Discount) / 100)));
        }
        public async Task<decimal> GetSaleValue(long accountId)
        {
            var discountAvg = await _context.Account.Include(d => d.Profile).ThenInclude(d => d.ProfileDiscounts)
                .Where(d => d.AccountId == accountId)
                .SelectMany(d => d.Profile.ProfileDiscounts)
                .AverageAsync(d => d.Discount);


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
            var paymentData = await _context.Payment
                              .Where(d => EF.Constant(accountIds).Contains(d.AccountId))
                              .GroupBy(d => d.AccountId)
                              .Select(d => new
                              {
                                  AccountId = d.Key,
                                  amountAirtime = d.Where(x => !new[] { (int)PaymentMethodType.USD, (int)PaymentMethodType.ZESA,
                                                      (int)PaymentMethodType.Nyaradzo, (int)PaymentMethodType.USDUtility }
                                                    .Contains(x.PaymentTypeId))
                                                     .Sum(x => x.Amount),
                                  amountZesa = d.Where(x => new[] { (int)PaymentMethodType.ZESA, (int)PaymentMethodType.Nyaradzo }
                                                 .Contains(x.PaymentTypeId)).Sum(x => x.Amount),
                                  AmountUSD = d.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USD)
                                  .Sum(x => x.Amount),
                                  AmountUSDUtility = d.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USDUtility)
                                  .Sum(x => x.Amount)
                              }).ToListAsync();

            //var paymentData = await (from p in _context.Payment.Where(d => EF.Constant(accountIds)
            //                         .Contains(d.AccountId))
            //                         group p by p.AccountId into g
            //                         select new
            //                         {
            //                             AccountId = g.Key,
            //                             amountAirtime = g.Where(x => !new[] { (int)PaymentMethodType.USD, (int)PaymentMethodType.ZESA, (int)PaymentMethodType.Nyaradzo, (int)PaymentMethodType.USDUtility }
            //                             .Contains(x.PaymentTypeId))
            //                             .Sum(x => x.Amount),
            //                             amountZesa = g.Where(x => new[] { (int)PaymentMethodType.ZESA, (int)PaymentMethodType.Nyaradzo }
            //                             .Contains(x.PaymentTypeId)).Sum(x => x.Amount),
            //                             AmountUSD = g.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USD).Sum(x => x.Amount),
            //                             AmountUSDUtility = g.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USDUtility).Sum(x => x.Amount)
            //                         }).ToListAsync();

            var rechargeData = await _context.Recharge.Include(d => d.Access).Include(d => d.Brand)
                                  .Where(d => EF.Constant(accountIds).Contains(d.Access.AccountId)
                                   && new[] { (int)SmsState.Busy, (int)SmsState.Success, (int)SmsState.PendingVerification }
                                   .Contains(d.StateId))
                                    .GroupBy(d => d.Access.AccountId)
                                    .Select(d => new
                                    {
                                        AccountId = d.Key,
                                        AmountAirtime = -d.Where(x => x.Brand.WalletTypeId == (int)WalletTypes.ZWG)
                                        .Sum(x => x.Amount * ((100 - x.Discount) / 100)),
                                        AmountZesa = -d.Where(x => x.Brand.WalletTypeId == (int)WalletTypes.ZESA)
                                        .Sum(x => x.Amount * ((100 - x.Discount) / 100)),
                                        AmountUSD = -d.Where(x => x.Brand.WalletTypeId == (int)WalletTypes.USD)
                                        .Sum(x => x.Amount * ((100 - x.Discount) / 100)),
                                        AmountUSDUtility = -d.Where(x => x.Brand.WalletTypeId == (int)WalletTypes.USDUtility)
                                        .Sum(x => x.Amount * ((100 - x.Discount) / 100))
                                    }).ToListAsync();

            //var rechargeData1 = await (from r in _context.Recharge
            //                          join a in _context.Access on r.AccessId equals a.AccessId
            //                          join b in _context.Brand on r.BrandId equals b.BrandId
            //                          where new[] { (int)SmsState.Busy, (int)SmsState.Success, (int)SmsState.PendingVerification }
            //                          .Contains(r.StateId)
            //                          group new { r, a, b } by a.AccountId into g
            //                          select new
            //                          {
            //                              AccountId = g.Key,
            //                              AmountAirtime = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.ZWG).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100)),
            //                              AmountZesa = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.ZESA).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100)),
            //                              AmountUSD = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.USD).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100)),
            //                              AmountUSDUtility = -g.Where(x => x.b.WalletTypeId == (int)WalletTypes.USDUtility).Sum(x => x.r.Amount * ((100 - x.r.Discount) / 100))
            //                          }).ToListAsync();

            var accountDiscounts = await _context.Account.Include(d => d.Profile).ThenInclude(d => d.ProfileDiscounts)
                                  .Where(d => EF.Constant(accountIds).Contains(d.AccountId))
                                  .GroupBy(d => d.AccountId)
                                   .Select(d => new
                                   {
                                       AccountId = d.Key,
                                       Discount = d.Sum(x => x.Profile.ProfileDiscounts.Average(y => y.Discount))
                                   }).ToListAsync();
            //var accountDiscounts = await (from a in _context.Account.Where(d => EF.Constant(accountIds)
            //                              .Contains(d.AccountId))
            //                              join pd in _context.ProfileDiscount on a.ProfileId equals pd.ProfileId
            //                              group pd by a.AccountId into g
            //                              select new
            //                              {
            //                                  AccountId = g.Key,
            //                                  Discount = g.Average(x => x.Discount)
            //                              }).ToListAsync();

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
        public async Task<List<ViewAccountModel>> GetViewAccountList(List<long> accountIds, int pageNo, int pageSize)
        {
            var bal = await GetViewBalanceList(accountIds);

            var accountList = await _context.Account.Include(d => d.Profile)
                .Where(d => EF.Constant(accountIds).Contains(d.AccountId))
                .Select(d => new
                {
                    d.AccountId,
                    d.Profile.ProfileId,
                    d.Profile.ProfileName,
                    d.AccountName,
                    d.NationalId,
                    d.Email,
                    d.ReferredBy
                }).ToListAsync();


            return (from a in accountList
                    join b in bal on a.AccountId equals b.AccountId into balances
                    from subB in balances.DefaultIfEmpty()
                    orderby subB.Balance descending
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
                    }).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
