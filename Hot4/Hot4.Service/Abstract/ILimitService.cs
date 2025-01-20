using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ILimitService
    {
        Task<bool> SaveLimit(LimitModel limit);
        Task<bool> UpdateLimit(LimitModel limit);
        Task<bool> DeleteLimit(LimitModel limit);
        Task<LimitModel?> GetLimitById(long limitId);
        Task<LimitPendingModel?> GetLimitByNetworkAndAccountId(int networkid, long accountid);
    }
}
