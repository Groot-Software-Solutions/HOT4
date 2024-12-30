using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IRechargePrepaidRepository
    {
        Task InsertRechargePrepaid(RechargePrepaid rechargeprepaid);
        Task UpdateRechargePrepaid(RechargePrepaid rechargePrepaid);

        Task<RechargePrepaidModel?> GetRechargePrepaid(long rechargeId);
        //  Task<AccountRechargePrepaidModel> SelectRechargePrepaid(long rechargeId);
        // Task<RechargePrepaid?> GetRechargeReversal(long rechargeId);
    }
}
