using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccessWebRepository
    {
        Task<AccessWeb?> GetAccessWebById(long accessId);
        Task<bool> AddAccessWeb(AccessWeb accessWeb);
        Task<bool> UpdateAccessWeb(AccessWeb accessWeb);
        Task<bool> DeleteAccessWeb(AccessWeb accessWeb);
    }
}
