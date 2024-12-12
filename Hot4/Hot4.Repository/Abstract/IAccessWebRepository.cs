using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAccessWebRepository
    {
        Task<AccessWeb?> GetAccessWeb(long accessId);

        Task AddAccessWeb(AccessWeb accessWeb);

        Task UpdateAccessWeb(AccessWeb accessWeb);
    }
}
