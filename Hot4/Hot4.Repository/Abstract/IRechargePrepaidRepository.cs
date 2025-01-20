using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IRechargePrepaidRepository
    {
        Task<bool> AddRechargePrepaid(RechargePrepaid rechargeprepaid);
        Task<bool> UpdateRechargePrepaid(RechargePrepaid rechargePrepaid);
        Task<RechargePrepaid?> GetRechargePrepaidById(long rechargeId);
    }
}
