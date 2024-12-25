using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
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

        public IQueryable<ViewBalanceModel> GetViewBalance()
        {
            return (from a in (
                       from p in _context.Payment
                       where !new[] { 17, 16, 18, 19 }.Contains(p.PaymentTypeId)
                       group p by p.AccountId into g
                       select new
                       {
                           AccountId = g.Key,
                           amountAirtime = g.Sum(x => x.Amount),
                           amountZesa = g.Where(x => new[] { (int)PaymentMethodType.ZESA, (int)PaymentMethodType.Nyaradzo }.Contains(x.PaymentTypeId)).Sum(x => x.Amount),
                           AmountUSD = g.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USD).Sum(x => x.Amount),
                           AmountUSDUtility = g.Where(x => x.PaymentTypeId == (int)PaymentMethodType.USDUtility).Sum(x => x.Amount)
                       }
                                      ).Union(
                            from r in _context.Recharge
                            join a1 in _context.Access on r.AccessId equals a1.AccessId
                            join b1 in _context.Brand on r.BrandId equals b1.BrandId
                            where new[] { (int)SmsState.Busy, (int)SmsState.Success, (int)SmsState.PendingVerification }.Contains(r.StateId)
                            group new { r, a1, b1 } by a1.AccountId into g
                            select new
                            {
                                AccountId = g.Key,
                                amountAirtime = g.Where(x => x.b1.WalletTypeId == (int)WalletTypes.ZWG).Sum(x => x.r.Amount * (1 - x.r.Discount / 100)),
                                amountZesa = g.Where(x => x.b1.WalletTypeId == (int)WalletTypes.ZESA).Sum(x => x.r.Amount * (1 - x.r.Discount / 100)),
                                AmountUSD = g.Where(x => x.b1.WalletTypeId == (int)WalletTypes.USD).Sum(x => x.r.Amount * (1 - x.r.Discount / 100)),
                                AmountUSDUtility = g.Where(x => x.b1.WalletTypeId == (int)WalletTypes.USDUtility).Sum(x => x.r.Amount * (1 - x.r.Discount / 100))
                            }
        )
                        // Join with discount data
                    join d in (
                             from y in _context.Account
                             join z in _context.ProfileDiscount on y.ProfileId equals z.ProfileId
                             group z by y.AccountId into g
                             select new
                             {
                                 AccountId = g.Key,
                                 Discount = g.Average(x => x.Discount)
                             }
                         ) on a.AccountId equals d.AccountId

                    group new { a, d } by new { a.AccountId, d.Discount } into finalGroup
                    select new ViewBalanceModel
                    {
                        AccountId = finalGroup.Key.AccountId,
                        Balance = finalGroup.Sum(x => x.a.amountAirtime),
                        ZESABalance = finalGroup.Sum(x => x.a.amountZesa),
                        USDBalance = finalGroup.Sum(x => x.a.AmountUSD),
                        USDUtilityBalance = finalGroup.Sum(x => x.a.AmountUSDUtility),
                        SaleValue = finalGroup.Sum(x => x.a.amountAirtime) / (1 - finalGroup.Key.Discount / 100)
                    }
                        );
        }
        public IQueryable<ViewAccountModel> GetViewAccount()
        {
            var bal = GetViewBalance();
            return from a in _context.Account.Include(d => d.Profile)

                   join b in bal on a.AccountId equals b.AccountId into balances
                   from b in balances.DefaultIfEmpty()
                   select new ViewAccountModel
                   {
                       AccountId = a.AccountId,
                       ProfileId = a.Profile.ProfileId,
                       ProfileName = a.Profile.ProfileName,
                       AccountName = a.AccountName,
                       NationalId = a.NationalId,
                       EmailId = a.Email,
                       RefferedBy = a.ReferredBy,
                       Balance = b != null ? b.Balance : 0,
                       SaleValue = b != null ? b.SaleValue : 0,
                       ZESABalance = b != null ? b.ZESABalance : 0,
                       USDBalance = b != null ? b.USDBalance : 0,
                       USDUtilityBalance = b != null ? b.USDUtilityBalance : 0
                   };
        }


    }
}
