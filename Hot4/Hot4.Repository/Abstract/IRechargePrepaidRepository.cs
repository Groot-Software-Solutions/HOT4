
using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRechargePrepaidRepository
    {
        Task InsertRechargePrepaid(RechargePrepaid rechargeprepaid);
        Task UpdateRechargePrepaid(RechargePrepaid rechargePrepaid);

        Task<RechargePrepaid?> GetRechargePrepaid(long rechargeId);
        Task<AccountRechargePrepaidModel> SelectRechargePrepaid(long rechargeId);
        Task<RechargePrepaid?> GetRechargeReversal(long rechargeId);
    }
}
