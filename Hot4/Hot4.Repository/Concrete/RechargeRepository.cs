using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class RechargeRepository : RepositoryBase<Recharge>, IRechargeRepository
    {
        public RechargeRepository(HotDbContext context) : base(context) { }
        public async Task<RechargeDetailModel?> GetRecharge(long rechargeId)
        {
            var result = await _context.Recharge.Include(d => d.State).Include(d => d.Brand).ThenInclude(d => d.Network).FirstOrDefaultAsync(d => d.RechargeId == rechargeId);
            if (result != null)
            {
                return new RechargeDetailModel
                {
                    AccessId = result.AccessId,
                    Amount = result.Amount,
                    BrandId = result.BrandId,
                    Mobile = result.Mobile,
                    BrandName = result.Brand.BrandName,
                    BrandSuffix = result.Brand.BrandSuffix,
                    InsertDate = result.InsertDate,
                    Discount = result.Discount,
                    Network = result.Brand.Network.Network,
                    NetworkId = result.Brand.NetworkId,
                    NetworkPrefix = result.Brand.Network.Prefix,
                    RechargeDate = result.RechargeDate,
                    RechargeId = result.RechargeId,
                    State = result.State.State,
                    StateId = result.StateId
                };
            }
            else
            {
                return null;
            }
        }
        public async Task InsertRecharge(Recharge recharge, long smsId)
        {
            await Create(recharge);
            await SaveChanges();

            if (recharge.RechargeId > 0 && smsId > 0)
            {
                await _context.AddAsync(new SmsRecharge
                {
                    RechargeId = recharge.RechargeId,
                    SmsId = smsId
                });
                await _context.SaveChangesAsync();
            }
        }
        public async Task UpdateRecharge(Recharge recharge)
        {
            await Update(recharge);
            await SaveChanges();

        }
        public async Task<List<RechargeModel>> GetRechargeAggregator(RechargeAggSearchModel rchAggSrch)
        {
            var startDate = rchAggSrch.StartDate.Date;
            var endDate = rchAggSrch.EndDate.Date.AddDays(1);

            var accessIds = await _context.Access.Where(d => d.AccountId == rchAggSrch.AccountId)
                .Select(d => d.AccessId).ToListAsync();

            var result = GetByCondition(d => d.RechargeDate >= startDate && d.RechargeDate < endDate
                           && EF.Constant(accessIds).Contains(d.AccessId))
                           .Include(d => d.Brand).Include(d => d.State);



            return await PaginationFilter.GetPagedData(result, rchAggSrch.PageNo, rchAggSrch.PageSize)
                .Queryable.Select(d => new RechargeModel
                {
                    RechargeId = d.RechargeId,
                    AccessId = d.AccessId,
                    Amount = d.Amount,
                    BrandId = d.BrandId,
                    Discount = d.Discount,
                    Mobile = d.Mobile,
                    RechargeDate = d.RechargeDate,
                    StateId = d.StateId

                }).ToListAsync();
        }
        public async Task<List<RechargeDetailModel>> RechargeFindByMobileAccountId(RechargeFindModel rechargeFind)
        {
            IQueryable<RechargeDetailModel> result;
            if (rechargeFind.AccountId > 0)
            {
                result = from r in _context.Recharge.Include(d => d.State).Include(d => d.Brand).ThenInclude(d => d.Network)
                         join acss in _context.Access on r.AccessId equals acss.AccessId
                         where r.Mobile.Contains(rechargeFind.Mobile) && acss.AccountId == rechargeFind.AccountId
                         select new RechargeDetailModel
                         {
                             AccessCode = acss.AccessCode,
                             AccessId = acss.AccessId,
                             Amount = r.Amount,
                             BrandId = r.BrandId,
                             BrandName = r.Brand.BrandName,
                             BrandSuffix = r.Brand.BrandSuffix,
                             Discount = r.Discount,
                             InsertDate = r.InsertDate,
                             Mobile = r.Mobile,
                             Network = r.Brand.Network.Network,
                             NetworkId = r.Brand.NetworkId,
                             NetworkPrefix = r.Brand.Network.Prefix,
                             RechargeDate = r.RechargeDate,
                             RechargeId = r.RechargeId,
                             State = r.State.State,
                             StateId = r.StateId
                         };
            }
            else
            {
                result = from r in _context.Recharge.Include(d => d.State).Include(d => d.Brand).ThenInclude(d => d.Network)
                         join acss in _context.Access on r.AccessId equals acss.AccessId
                         where r.Mobile.Contains(rechargeFind.Mobile)
                         select new RechargeDetailModel
                         {
                             AccessCode = acss.AccessCode,
                             AccessId = acss.AccessId,
                             Amount = r.Amount,
                             BrandId = r.BrandId,
                             BrandName = r.Brand.BrandName,
                             BrandSuffix = r.Brand.BrandSuffix,
                             Discount = r.Discount,
                             InsertDate = r.InsertDate,
                             Mobile = r.Mobile,
                             Network = r.Brand.Network.Network,
                             NetworkId = r.Brand.NetworkId,
                             NetworkPrefix = r.Brand.Network.Prefix,
                             RechargeDate = r.RechargeDate,
                             RechargeId = r.RechargeId,
                             State = r.State.State,
                             StateId = r.StateId
                         };
            }

            return await PaginationFilter.GetPagedData(result, rechargeFind.PageNo, rechargeFind.PageSize).Queryable.ToListAsync();
        }
        public async Task<List<RechargeModel>> RechargePending(List<byte> brandIds)
        {

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rechargeIds = await GetByCondition(d => d.StateId == (int)SmsState.Pending
                                    && EF.Constant(brandIds).Contains(d.BrandId))
                    .OrderBy(d => d.RechargeId).Select(d => d.RechargeId).Take(300).ToListAsync();


                var rechargesToUpdate = await GetByCondition(d => EF.Constant(rechargeIds).Contains(d.RechargeId)).ToListAsync();

                foreach (var recharge in rechargesToUpdate)
                {
                    recharge.StateId = (int)SmsState.Busy;
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return rechargesToUpdate.Select(d => new RechargeModel
                {
                    RechargeId = d.RechargeId,
                    AccessId = d.AccessId,
                    Amount = d.Amount,
                    BrandId = d.BrandId,
                    Discount = d.Discount,
                    Mobile = d.Mobile,
                    RechargeDate = d.RechargeDate,
                    StateId = d.StateId
                }).ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }

        public async Task<RechargeDetailModel?> RechargePendingByBrandId(byte brandId)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rechargeId = await _context.Recharge
                                        .Where(d => d.StateId == (int)SmsState.Pending
                                        && d.BrandId == brandId)
                                        .OrderBy(d => d.RechargeId)
                                        .Select(d => d.RechargeId)
                                        .FirstOrDefaultAsync();

                if (rechargeId != 0)
                {
                    var rechargeToUpdate = await _context.Recharge
                      .FirstOrDefaultAsync(d => d.RechargeId == rechargeId);
                    if (rechargeToUpdate != null)
                    {
                        rechargeToUpdate.StateId = (int)SmsState.Busy;
                        await _context.SaveChangesAsync();
                    }

                    var result = await _context.Recharge.Include(d => d.State).Include(d => d.Brand).ThenInclude(d => d.Network).FirstOrDefaultAsync(d => d.RechargeId == rechargeId);
                    if (result != null)
                    {
                        return new RechargeDetailModel
                        {
                            AccessId = result.AccessId,
                            Amount = result.Amount,
                            BrandId = result.BrandId,
                            Mobile = result.Mobile,
                            BrandName = result.Brand.BrandName,
                            BrandSuffix = result.Brand.BrandSuffix,
                            InsertDate = result.InsertDate,
                            Discount = result.Discount,
                            Network = result.Brand.Network.Network,
                            NetworkId = result.Brand.NetworkId,
                            NetworkPrefix = result.Brand.Network.Prefix,
                            RechargeDate = result.RechargeDate,
                            RechargeId = result.RechargeId,
                            State = result.State.State,
                            StateId = result.StateId
                        };
                    }

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
            return null;
        }
        public async Task<RechargeDetailModel?> RechargePendingOtherBrand()
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rechargeId = await _context.Recharge
                                        .Where(d => d.StateId == (int)SmsState.Pending
                                        && new[] { (int)Brands.EasyCall, (int)Brands.Juice, (int)Brands.Econet078 }.Contains(d.BrandId))
                                        .OrderBy(d => d.RechargeId)
                                        .Select(d => d.RechargeId)
                                        .FirstOrDefaultAsync();

                if (rechargeId != 0)
                {
                    var rechargeToUpdate = await _context.Recharge
                      .FirstOrDefaultAsync(d => d.RechargeId == rechargeId);
                    if (rechargeToUpdate != null)
                    {
                        rechargeToUpdate.StateId = (int)SmsState.Busy;
                        await _context.SaveChangesAsync();
                    }

                    var result = await _context.Recharge.Include(d => d.State).Include(d => d.Brand).ThenInclude(d => d.Network).FirstOrDefaultAsync(d => d.RechargeId == rechargeId);
                    if (result != null)
                    {
                        return new RechargeDetailModel
                        {
                            AccessId = result.AccessId,
                            Amount = result.Amount,
                            BrandId = result.BrandId,
                            Mobile = result.Mobile,
                            BrandName = result.Brand.BrandName,
                            BrandSuffix = result.Brand.BrandSuffix,
                            InsertDate = result.InsertDate,
                            Discount = result.Discount,
                            Network = result.Brand.Network.Network,
                            NetworkId = result.Brand.NetworkId,
                            NetworkPrefix = result.Brand.Network.Prefix,
                            RechargeDate = result.RechargeDate,
                            RechargeId = result.RechargeId,
                            State = result.State.State,
                            StateId = result.StateId
                        };
                    }

                    await transaction.CommitAsync();
                }
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
            return null;
        }

    }
}
