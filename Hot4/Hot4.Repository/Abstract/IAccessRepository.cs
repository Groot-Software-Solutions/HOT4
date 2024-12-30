using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccessRepository
    {
        Task<AccessModel?> GetAccess(long accessId);
        Task<List<AccessModel>> ListAccountChannel(long accountId, byte channelId);
        Task<AccountAccessModel?> GetByAccessCode(string accessCode);
        Task AddAccess(Access access);
        Task UpdateAccess(Access access);
        Task<List<AccountAccessModel>> ListAccess(long accountId, bool isGetAll, bool isDeleted);
        Task<long> GetAdminId(long accountId);
        Task PasswordChange(long accessId, string newPassword);
        Task<AccountAccessModel?> GetLoginDetails(string accessCode, string accessPassword);
        Task DeleteAccess(long accessId);
        Task UnDeleteAccess(long accessId);
    }
}
