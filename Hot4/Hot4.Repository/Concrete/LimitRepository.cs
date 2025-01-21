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
        public async Task<bool> SaveLimit(Limit limit)
        {
            await Create(limit);
            await SaveChanges();
            return true;
            //var limitExist = await _context.Limit.FirstOrDefaultAsync(d => d.AccountId == limit.AccountId && d.NetworkId == limit.NetworkId);
            //if (limitExist == null)
            //{
            //    await Create(limit);
            //    await SaveChanges();
            //    return limit.LimitId;
            //}
            //else
            //{
            //    limitExist.AccountId = limit.AccountId;
            //    limitExist.DailyLimit = limit.DailyLimit;
            //    limitExist.LimitTypeId = limit.LimitTypeId;
            //    limitExist.MonthlyLimit = limit.MonthlyLimit;
            //    limitExist.NetworkId = limit.NetworkId;
            //    Update(limit);
            //    await SaveChanges();
            //    return limitExist.LimitId;
            //}
        }
        public async Task<bool> UpdateLimit(Limit limit)
        {
            Update(limit);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteLimit(Limit limit)
        {
            Delete(limit);
            await SaveChanges();
            return true;
        }
        public async Task<Limit?> GetLimitById(long limitId)
        {

            return await GetById(limitId);

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

            var startDateMonthly = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var startDateDaily = DateTime.Today;

            // Get daily sales
            salesDaily = (float)await _context.Recharge.Include(d => d.Access).Include(d => d.Brand)
                        .Where(d => d.RechargeDate >= startDateDaily && d.RechargeDate <= DateTime.Now
                        && new[] { (int)SmsState.Busy, (int)SmsState.Success }.Contains(d.StateId)
                        && d.Access.AccountId == accountid
                        && d.Brand.WalletTypeId == (int)WalletTypes.ZWG
                        && (d.Brand.NetworkId == networkid ||
                              (networkid == (int)NetworkName.Econet ? d.Brand.NetworkId == (int)NetworkName.Econet078 : false))
                         ).SumAsync(d => d.Amount);


            //var salesDaily = (float)await (from r in _context.Recharge
            //                                where r.RechargeDate >= startDateDaily && r.RechargeDate <= DateTime.Now
            //                                && new[] { (int)SmsState.Busy, (int)SmsState.Success }.Contains(r.StateId)
            //                                join a in _context.Access on r.AccessId equals a.AccessId
            //                                where a.AccountId == accountid
            //                                join b in _context.Brand on r.BrandId equals b.BrandId
            //                                where (b.NetworkId == networkid || (
            //                                (networkid == (int)NetworkName.Econet ? b.NetworkId == (int)NetworkName.Econet078 : false)
            //                                ))
            //                                && b.WalletTypeId == (int)WalletTypes.ZWG
            //                                select r.Amount).SumAsync();

            // Get monthly sales
            salesMonthly = (float)await _context.Recharge.Include(d => d.Access).Include(d => d.Brand)
                      .Where(d => d.RechargeDate >= startDateMonthly && d.RechargeDate <= DateTime.Now
                      && new[] { (int)SmsState.Busy, (int)SmsState.Success }.Contains(d.StateId)
                      && d.Access.AccountId == accountid
                      && d.Brand.WalletTypeId == (int)WalletTypes.ZWG
                      && (d.Brand.NetworkId == networkid ||
                            (networkid == (int)NetworkName.Econet ? d.Brand.NetworkId == (int)NetworkName.Econet078 : false))
                       ).SumAsync(d => d.Amount);

            //salesMonthly = (float)await (from r in _context.Recharge
            //                             where r.RechargeDate >= startDateMonthly && r.RechargeDate <= DateTime.Now
            //                             && new[] { (int)SmsState.Busy, (int)SmsState.Success }.Contains(r.StateId)
            //                             join a in _context.Access on r.AccessId equals a.AccessId
            //                             where a.AccountId == accountid
            //                             join b in _context.Brand on r.BrandId equals b.BrandId
            //                             where (b.NetworkId == networkid ||
            //                             (networkid == (int)NetworkName.Econet ? b.NetworkId == (int)NetworkName.Econet078 : false))
            //                             && b.WalletTypeId == (int)WalletTypes.ZWG
            //                             select r.Amount).SumAsync();

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


    }
}
