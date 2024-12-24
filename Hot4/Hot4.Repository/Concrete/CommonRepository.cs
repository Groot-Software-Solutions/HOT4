using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class CommonRepository : ICommonRepository
    {
        private HotDbContext _context { get; set; }
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


    }
}
