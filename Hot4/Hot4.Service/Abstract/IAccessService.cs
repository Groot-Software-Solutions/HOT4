using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IAccessService
    {
        Task<AccessModel?> GetAccessById(long accessId);
        Task<List<AccessModel>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId);
        Task<AccountAccessModel?> GetAccessByCode(string accessCode);

        Task<bool> AddAccess(AccessModel accessModel );
        Task<bool> AddAccessDeprecated(AccessModel accessModel);
        Task<bool> UpdateAccess(AccessModel accessModel);

        Task<List<AccountAccessModel>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted);

        Task<long> GetAdminId(long accountId);

        Task<bool> PasswordChange(long accessId, string newPassword);

        Task<bool> PasswordChangeDeprecated(long accessId, string passwordHash, string passwordSalt);

        Task<AccountAccessModel?> GetLoginDetails(string accessCode, string accessPassword);

        Task<AccountAccessModel?> GetLoginDetailsByAccessCode(string accessCode);

        Task<bool> DeleteAccess(long accessId);
        Task<bool> UnDeleteAccess(long accessId);

    }
}
