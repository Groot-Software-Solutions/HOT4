using Hot4.Core.DataViewModels;

using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAccessRepository
    {
        Task<Access?> GetAccess(long accessId);
        Task<List<Access>> ListAccountChannel(long accountId, byte channelId);
        Task<Access?> GetByAccessCode(string accessCode);
        Task AddAccess(Access access);
        Task UpdateAccess(Access access);
        Task<List<AccountAccessModel>> ListAccess(long accountId, bool isGetAll, bool isDeleted);

        bool SMSLoginIsValid(Access access, string password);

        Task<long> GetAdminIDAsync(long accountId);
    }
}
