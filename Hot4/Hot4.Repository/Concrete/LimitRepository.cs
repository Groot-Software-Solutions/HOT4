﻿using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class LimitRepository : RepositoryBase<Limit>, ILimitRepository
    {
        public LimitRepository(HotDbContext context) : base(context)
        {

        }
        public async Task<long> SaveUpdateLimit(Limit limit)
        {
            var limitExist = await GetByCondition(d => d.AccountId == limit.AccountId && d.NetworkId == limit.NetworkId).FirstOrDefaultAsync();
            if (limitExist == null)
            {
                await Create(limit);
                await SaveChanges();
                return limit.LimitId;
            }
            else
            {
                await Update(limit);
                await SaveChanges();
                return limit.LimitId;
            }
        }


        public async Task<LimitModel?> GetLimit(long limitId)
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
        public async Task<LimitPendingModel> GetLimitByNetAccountId(int networkid, long accountid)
        {
            if (networkid == (int)NetworkName.Econet078)
            {
                networkid = (int)NetworkName.Econet;
            }

            float? dailylimit = 0, montlylimit = 0, salesDaily = 0, salesMonthly = 0;
            int limitTypeid = (int)LimitTypeName.Yearly;

            var limits = await (from limit in _context.Limit
                                where limit.NetworkId == networkid && limit.AccountId == accountid
                                select new { limit.DailyLimit, limit.MonthlyLimit, limit.LimitTypeId }).FirstOrDefaultAsync();

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
                                       join a in _context.Access on r.AccessId equals a.AccessId
                                       join b in _context.Brand on r.BrandId equals b.BrandId
                                       where r.RechargeDate >= startDateDaily && r.RechargeDate <= DateTime.Now
                                       && (r.StateId == (int)SmsState.Busy || r.StateId == (int)SmsState.Success)
                                       && (b.NetworkId == networkid || (networkid == (int)NetworkName.Econet && b.NetworkId == (int)NetworkName.Econet078))
                                       && b.WalletTypeId == (int)WalletTypes.ZWG && a.AccountId == accountid
                                       select r.Amount).SumAsync();

            // Get monthly sales
            salesMonthly = (float)await (from r in _context.Recharge
                                         join a in _context.Access on r.AccessId equals a.AccessId
                                         join b in _context.Brand on r.BrandId equals b.BrandId
                                         where r.RechargeDate >= startDateMonthly && r.RechargeDate <= DateTime.Now
                                         && (r.StateId == (int)SmsState.Busy || r.StateId == (int)SmsState.Success)
                                         && (b.NetworkId == networkid || (networkid == (int)NetworkName.Econet && b.NetworkId == (int)NetworkName.Econet078))
                                         && b.WalletTypeId == (int)WalletTypes.ZWG && a.AccountId == accountid
                                         select r.Amount).SumAsync();

            // Calculating remaining limits
            float remainingMonthlyLimit = (montlylimit ?? 10000) - (salesMonthly ?? 0);
            float remainingDailyLimit = (dailylimit ?? 1000) - (salesDaily ?? 0);

            // Set the remaining limit logic
            float remainingLimit = remainingDailyLimit;
            if ((montlylimit ?? 10000) - (salesMonthly ?? 0) < remainingLimit)
            {
                remainingLimit = (montlylimit ?? 10000) - (salesMonthly ?? 0);
            }

            return new LimitPendingModel
            {

                LimitRemaining = remainingLimit,
                RemainingLimit = remainingLimit,
                RemainingDailyLimit = remainingDailyLimit,
                RemainingMonthlyLimit = remainingMonthlyLimit,
                DailyLimit = dailylimit ?? 1000,
                MonthlyLimit = montlylimit ?? 10000,
                SalesToday = salesDaily ?? 0,
                SalesMonthly = salesMonthly ?? 0,
                LimitTypeId = limitTypeid,
                NetworkId = networkid
            };
        }



    }
}
