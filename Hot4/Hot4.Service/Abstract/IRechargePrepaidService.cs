using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IRechargePrepaidService
    {
        Task<bool> AddRechargePrepaid(RechargePrepaidModel rechargeprepaid);
        Task<bool> UpdateRechargePrepaid(RechargePrepaidModel rechargePrepaid);
        Task<RechargePrepaidModel?> GetRechargePrepaidById(long rechargeId);
    }
}
