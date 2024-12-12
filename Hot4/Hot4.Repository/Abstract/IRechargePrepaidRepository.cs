using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRechargePrepaidRepository
    {
        Task InsertRechargePrepaid(TblRechargePrepaid rechargeprepaid);
        Task UpdateRechargePrepaid(TblRechargePrepaid rechargePrepaid);

        Task<TblRechargePrepaid?> GetRechargePrepaid(long rechargeId);
        Task<AccountRechargePrepaidModel> SelectRechargePrepaid(long rechargeId);
        Task<TblRechargePrepaid?> GetRechargeReversal(long rechargeId);
    }
}
