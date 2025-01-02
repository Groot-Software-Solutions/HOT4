using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ILimitRepository
    {
        Task<long> SaveUpdateLimit(Limit limit);
        Task<LimitModel?> GetLimitById(long limitId);
        Task<LimitPendingModel> GetLimitByNetworkAndAccountId(int networkid, long accountid);
        Task DeleteLimit(Limit limit);
    }
}
