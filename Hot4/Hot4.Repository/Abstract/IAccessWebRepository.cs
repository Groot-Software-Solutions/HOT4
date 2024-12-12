using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAccessWebRepository
    {
        Task<TblAccessWeb?> GetAccessWeb(long accessId);

        Task AddAccessWeb(TblAccessWeb accessWeb);

        Task UpdateAccessWeb(TblAccessWeb accessWeb);
    }
}
