using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class RechargeRepository : RepositoryBase<Recharge>, IRechargeRepository
    {
        public RechargeRepository(HotDbContext context) : base(context) { }
        public async Task<Recharge?> GetRecharge(long rechargeId)
        {
            return await GetById(rechargeId);
        }
        public async Task<Recharge?> InsertAndGetIdentity(Recharge recharge)
        {
            await Create(recharge);
            await SaveChanges();
            return await GetById(recharge.RechargeId);
        }
        public async Task UpdateRecharge(Recharge recharge)
        {
            await Update(recharge);
            await SaveChanges();

        }
        //public async Task<List<AccountRechargePinModel>> ListRechargePin(long rechargeId)
        //{
        //    return await (from pin in _context.Pin
        //                  join rechPin in _context.RechargePin
        //                  on pin.PinId equals rechPin.PinId
        //                  join rech in _context.Recharge
        //                  on rechPin.RechargeId equals rech.RechargeId
        //                  where rech.RechargeId == rechargeId
        //                  select new AccountRechargePinModel()
        //                  {
        //                      //ProductID = pin.ProductId,
        //                      PinBatchID = pin.PinBatchId,
        //                      Pin = pin.Pin,
        //                      PinExpiry = pin.PinExpiry,
        //                      PinID = pin.PinId,
        //                      PinRef = pin.PinRef,
        //                      PinStateID = pin.PinStateId,
        //                      PinValue = pin.PinValue,
        //                  }).ToListAsync();
        //}

        public async Task<List<Recharge>> GetPendingRechargesWithTransaction(int takeRows)
        {
            var rechargeList = new List<Recharge>();
            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();

                rechargeList = await _context.Recharge
                    .Where(d => d.StateId == (byte)SmsState.Pending)
                    .OrderBy(d => d.RechargeDate)
                    .Take(takeRows)
                    .ToListAsync();
                if (rechargeList != null && rechargeList.Count > 0)
                {
                    rechargeList.ForEach(d => d.StateId = (byte)SmsState.Busy);

                    _context.UpdateRange(rechargeList);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                else
                {

                    await transaction.RollbackAsync();
                }
            }
            catch
            {
                if (_context.Database.CurrentTransaction != null)
                {
                    await _context.Database.RollbackTransactionAsync();
                }

                throw;
            }

            return rechargeList ?? new List<Recharge>();
        }


        //public async Task<List<RechargeAccess>> ListRechargeForMobile(string mobile, DateTime date)
        //{
        //    return await (from rech in _context.Recharge
        //                  join acss in _context.Access on rech.AccessId equals acss.AccessId
        //                  join chn in _context.Channel on acss.ChannelId equals chn.ChannelId
        //                  join state in _context.State on rech.StateId equals state.StateId
        //                  where EF.Constant(rech.Mobile).Contains(mobile)
        //                  orderby rech.RechargeId descending
        //                  select new RechargeAccess()
        //                  {
        //                      Discount = rech.Discount,
        //                      AccessID = acss.AccessId,
        //                      Amount = rech.Amount, //.SalesAmount,
        //                      Mobile = rech.Mobile,
        //                      RechargeID = rech.RechargeId,
        //                      State = state.State,
        //                      InsertDate = rech.InsertDate,
        //                      AccessChannel = chn.Channel,
        //                      AccessCode = acss.AccessCode,
        //                      IsSuccessFul = (rech.StateId == (byte)SmsStatus.Success)
        //                  }
        //                  ).Take(20).ToListAsync();
        //}


    }
}
