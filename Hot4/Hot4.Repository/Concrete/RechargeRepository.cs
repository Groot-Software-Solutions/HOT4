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
    public class RechargeRepository : RepositoryBase<Recharge>, IRechargeRepository
    {
        private ValueSettings _valueSettings { get; }
        public RechargeRepository(HotDbContext context, IOptions<ValueSettings> valueSetting) : base(context)
        {
            _valueSettings = valueSetting.Value;
        }
        public async Task<Recharge?> GetRechargeById(long rechargeId)
        {
            return await _context.Recharge
                              .Include(d => d.State)
                              .Include(d => d.Brand)
                              .ThenInclude(d => d.Network)
                              .FirstOrDefaultAsync(d => d.RechargeId == rechargeId);
        }
        public async Task<bool> AddRecharge(Recharge recharge, long smsId)
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
                return true;
            }
            return false;
        }
        public async Task<bool> AddRechargeWithOutSmsDetails(Recharge recharge)
        {
            await Create(recharge);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateRecharge(Recharge recharge)
        {
            Update(recharge);
            await SaveChanges();
            return true;
        }

        public async Task<bool> DeleteRecharge(Recharge recharge)
        {
            Delete(recharge);
            await SaveChanges();
            return true;
        }

        public async Task<List<Recharge>> FindRechargeByMobileAndAccountId(RechargeFindModel rechargeFind)
        {
            IQueryable<Recharge> result;
            if (rechargeFind.AccountId > 0)
            {
                result = _context.Recharge.Include(d => d.State).Include(d => d.Access)
                         .Include(d => d.Brand).ThenInclude(d => d.Network)
                         .Where(d => d.Mobile.Contains(rechargeFind.Mobile)
                           && d.Access.AccountId == rechargeFind.AccountId)
                         .OrderBy(d => d.RechargeId);


            }
            else
            {
                result = _context.Recharge.Include(d => d.State).Include(d => d.Access)
                         .Include(d => d.Brand).ThenInclude(d => d.Network)
                         .Where(d => d.Mobile.Contains(rechargeFind.Mobile))
                         .OrderBy(d => d.RechargeId);
            }

            return await PaginationFilter.GetPagedData(result, rechargeFind.PageNo, rechargeFind.PageSize).Queryable.ToListAsync();
        }
        public async Task<List<Recharge>> RechargePendingStsByMulBrands(List<byte> brandIds)
        {

            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var rechargesToUpdate = await GetByCondition(d => d.StateId == (int)SmsState.Pending
                                    && EF.Constant(brandIds).Contains(d.BrandId))
                    .OrderBy(d => d.RechargeId)
                    .Take(_valueSettings.RechargeIdByBrandIds)
                    .ToListAsync();

                foreach (var recharge in rechargesToUpdate)
                {
                    recharge.StateId = (int)SmsState.Busy;
                }
                _context.Recharge.UpdateRange(rechargesToUpdate);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return rechargesToUpdate;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<Recharge?> RechargePendingStsByBrandId(byte brandId)
        {

            var result = await _context.Recharge
                                   .Include(d => d.Brand)
                                   .ThenInclude(d => d.Network)
                                   .FirstOrDefaultAsync(d => d.StateId == (int)SmsState.Pending
                                    && d.BrandId == brandId);

            if (result != null)
            {
                result.StateId = (int)SmsState.Busy;
                _context.Update(result);
                await _context.SaveChangesAsync();
                return result;
            }

            return null;
        }
        public async Task<Recharge?> GetRechargeWebDuplicate(RechWebDupSearchModel rechWebDup)
        {
            return await _context.Recharge.FirstOrDefaultAsync(d => d.AccessId == rechWebDup.AccessId
                        && d.Mobile == rechWebDup.Mobile
                        && d.Amount == rechWebDup.Amount
                        && d.InsertDate > DateTime.Now.AddMinutes(-1));

        }
    }
}
