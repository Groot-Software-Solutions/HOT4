using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccessWebRepository
    {
        Task<AccessWeb?> GetAccessWebById(long accessId);
        Task AddAccessWeb(AccessWeb accessWeb);
        Task UpdateAccessWeb(AccessWeb accessWeb);
        Task DeleteAccessWeb(AccessWeb accessWeb);
    }
}
