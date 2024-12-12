using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ILimitRepository
    {
        Task<Limit?> GetLimit(long limitId);
        Task<long> AddLimit(Limit limit);
        Task UpdateLimit(Limit limit);
        Task DeleteLimit(long limitId);
    }
}
