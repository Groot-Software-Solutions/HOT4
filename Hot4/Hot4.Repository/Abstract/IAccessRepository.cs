using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccessRepository
    {
        Task<Access?> GetAccessById(long accessId);
        Task<List<Access>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId);
        Task<Access?> GetAccessByCode(string accessCode);
        Task AddAccess(Access access);
        Task AddAccessDeprecated(Access access);
        Task UpdateAccess(Access access);
        Task<List<Access>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted);
        Task<long> GetAdminId(long accountId);
        Task PasswordChange(long accessId, string newPassword);
        Task PasswordChangeDeprecated(long accessId, string passwordHash, string passwordSalt);
        Task<Access?> GetLoginDetails(string accessCode, string accessPassword);
        Task<Access?> GetLoginDetailsByAccessCode(string accessCode);
        Task DeleteAccess(long accessId);
        Task UnDeleteAccess(long accessId);
    }
}
