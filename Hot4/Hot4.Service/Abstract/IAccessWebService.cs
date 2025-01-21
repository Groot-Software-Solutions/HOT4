using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IAccessWebService
    {
        Task<AccessWebModel?> GetAccessWebById(long accessId);
        Task<bool> AddAccessWeb(AccessWebModel accessWebModel);
        Task <bool>UpdateAccessWeb(AccessWebModel accessWebModel);
        Task<bool> DeleteAccessWeb(long AccessId);
    }
}
