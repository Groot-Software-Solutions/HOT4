using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccessRepository
    {
        Task<Access?> GetAccessById(long accessId);
        Task<List<Access>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId);
        Task<Access?> GetAccessByCode(string accessCode);
        Task<bool> AddAccess(Access access);
        Task<bool> AddAccessDeprecated(Access access);
        Task<bool> UpdateAccess(Access access);
        Task<List<Access>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted);
        Task<long> GetAdminId(long accountId);
        Task<bool> PasswordChange(Access access,long accessId,string newPassword);
        Task<bool> PasswordChangeDeprecated(Access access, string passwordHash, string passwordSalt);
        Task<Access?> GetLoginDetails(string accessCode, string accessPassword);
        Task<Access?> GetLoginDetailsByAccessCode(string accessCode);
        Task<bool> DeleteAccess(Access access);
        Task<bool> UnDeleteAccess(Access access);
    }
}
