using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IRechargePrepaidRepository
    {
        Task AddRechargePrepaid(RechargePrepaid rechargeprepaid);
        Task UpdateRechargePrepaid(RechargePrepaid rechargePrepaid);
        Task<RechargePrepaidModel?> GetRechargePrepaidById(long rechargeId);
    }
}
