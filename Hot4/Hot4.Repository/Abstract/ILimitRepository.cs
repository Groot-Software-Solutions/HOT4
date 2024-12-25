using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface ILimitRepository
    {
        Task<long> SaveUpdateLimit(Limit limit);
        Task<LimitModel?> GetLimit(long limitId);
        Task<LimitPendingModel> GetLimitByNetAccountId(int networkid, long accountid);
        Task DeleteLimit(Limit limit);

    }
}
