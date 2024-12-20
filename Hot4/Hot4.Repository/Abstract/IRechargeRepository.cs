
using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRechargeRepository
    {

        Task<List<AccountRechargePinModel>> ListRechargePin(long rechargeId);

        Task<Recharge?> GetRecharge(long rechargeId);

        Task UpdateRecharge(Recharge recharge);

        Task<Recharge?> InsertAndGetIdentity(Recharge recharge);

        // Task<List<RechargeAccess>> ListRechargeForMobile(string mobile, DateTime date);

        Task<List<Recharge>> GetPendingRechargesWithTransaction(int takeRows);

    }
}
