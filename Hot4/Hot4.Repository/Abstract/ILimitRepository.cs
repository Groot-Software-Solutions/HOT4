using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ILimitRepository
    {
        Task<bool> SaveLimit(Limit limit);
        Task<bool> UpdateLimit(Limit limit);
        Task<bool> DeleteLimit(Limit limit);
        Task<Limit?> GetLimitById(long limitId);
        Task<LimitPendingModel> GetLimitByNetworkAndAccountId(int networkid, long accountid);

    }
}
