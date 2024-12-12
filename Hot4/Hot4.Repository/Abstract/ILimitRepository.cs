using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ILimitRepository
    {
        Task<TblLimit?> GetLimit(long limitId);
        Task<long> AddLimit(TblLimit limit);
        Task UpdateLimit(TblLimit limit);
        Task DeleteLimit(long limitId);
    }
}
