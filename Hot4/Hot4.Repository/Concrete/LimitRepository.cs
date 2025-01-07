using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class LimitRepository : RepositoryBase<Limit>, ILimitRepository
    {
        public LimitRepository(HotDbContext context) : base(context) { }
        public async Task<long> SaveUpdateLimit(Limit limit)
        {
            var limitExist = await _context.Limit.FirstOrDefaultAsync(d => d.AccountId == limit.AccountId && d.NetworkId == limit.NetworkId);
            if (limitExist == null)
            {
                await Create(limit);
                await SaveChanges();
                return limit.LimitId;
            }
            else
            {
                limitExist.AccountId = limit.AccountId;
                limitExist.DailyLimit = limit.DailyLimit;
                limitExist.LimitTypeId = limit.LimitTypeId;
                limitExist.MonthlyLimit = limit.MonthlyLimit;
                limitExist.NetworkId = limit.NetworkId;
                Update(limit);
                await SaveChanges();
                return limitExist.LimitId;
            }
        }
        public async Task<LimitModel?> GetLimitById(long limitId)
        {

            var res = await GetById(limitId);
            if (res != null)
            {
                return new LimitModel
                {
                    AccountId = res.AccountId,
                    DailyLimit = res.DailyLimit,
                    LimitId = res.LimitId,
                    LimitTypeId = res.LimitTypeId,
                    MonthlyLimit = res.MonthlyLimit,
                    NetworkId = res.NetworkId,
                };
            }
            return null;
        }
        public async Task<LimitPendingModel> GetLimitByNetworkAndAccountId(int networkid, long accountid)
        {
            if (networkid == (int)NetworkName.Econet078)
            {
                networkid = (int)NetworkName.Econet;
            }

            float? dailylimit = 0, montlylimit = 0, salesDaily = 0, salesMonthly = 0;
            int limitTypeid = (int)LimitTypeName.Yearly;
            var limits = await _context.Limit.FirstOrDefaultAsync(d => d.NetworkId == networkid && d.AccountId == accountid);
            if (limits != null)
            {
                dailylimit = (float)limits.DailyLimit;
                montlylimit = (float)limits.MonthlyLimit;
                limitTypeid = limits.LimitTypeId;
            }

            // Setting date ranges for daily and monthly limits
            var startDateMonthly = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startDateDaily = DateTime.Today;

            // Get daily sales
            salesDaily = (float)await (from r in _context.Recharge
                                       where r.RechargeDate >= startDateDaily && r.RechargeDate <= DateTime.Now
                                       && new[] { (int)SmsState.Busy, (int)SmsState.Success }.Contains(r.StateId)
                                       join a in _context.Access on r.AccessId equals a.AccessId
                                       where a.AccountId == accountid
                                       join b in _context.Brand on r.BrandId equals b.BrandId
                                       where (b.NetworkId == networkid || (networkid == (int)NetworkName.Econet &&
                                       b.NetworkId == (int)NetworkName.Econet078))
                                       && b.WalletTypeId == (int)WalletTypes.ZWG
                                       select r.Amount).SumAsync();

            // Get monthly sales
            salesMonthly = (float)await (from r in _context.Recharge
                                         where r.RechargeDate >= startDateMonthly && r.RechargeDate <= DateTime.Now
                                         && new[] { (int)SmsState.Busy, (int)SmsState.Success }.Contains(r.StateId)
                                         join a in _context.Access on r.AccessId equals a.AccessId
                                         where a.AccountId == accountid
                                         join b in _context.Brand on r.BrandId equals b.BrandId
                                         where (b.NetworkId == networkid || (networkid == (int)NetworkName.Econet &&
                                         b.NetworkId == (int)NetworkName.Econet078))
                                         && b.WalletTypeId == (int)WalletTypes.ZWG
                                         select r.Amount).SumAsync();

            return new LimitPendingModel
            {
                DailyLimit = dailylimit,
                MonthlyLimit = montlylimit,
                SalesToday = salesDaily ?? 0,
                SalesMonthly = salesMonthly ?? 0,
                LimitTypeId = limitTypeid,
                NetworkId = networkid
            };
        }

        public async Task DeleteLimit(Limit limit)
        {
            Delete(limit);
            await SaveChanges();
        }
    }
}
