using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRechargeRepository
    {

        Task<List<AccountRechargePinModel>> ListRechargePin(long rechargeId);

        Task<TblRecharge?> GetRecharge(long rechargeId);

        Task UpdateRecharge(TblRecharge recharge);

        Task<TblRecharge?> InsertAndGetIdentity(TblRecharge recharge);

        Task<List<RechargeAccess>> ListRechargeForMobile(string mobile, DateTime date);

        Task<List<TblRecharge>> GetPendingRechargesWithTransaction(int takeRows);

    }
}
