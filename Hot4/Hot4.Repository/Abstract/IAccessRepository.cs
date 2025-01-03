﻿using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccessRepository
    {
        Task<AccessModel?> GetAccessById(long accessId);
        Task<List<AccessModel>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId);
        Task<AccountAccessModel?> GetAccessByCode(string accessCode);
        Task AddAccess(Access access);
        Task AddAccessDeprecated(Access access);
        Task UpdateAccess(Access access);
        Task<List<AccountAccessModel>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted);
        Task<long> GetAdminId(long accountId);
        Task PasswordChange(long accessId, string newPassword);
        Task PasswordChangeDeprecated(long accessId, string passwordHash, string passwordSalt);
        Task<AccountAccessModel?> GetLoginDetails(string accessCode, string accessPassword);
        Task<AccountAccessModel?> GetLoginDetailsByAccessCode(string accessCode);
        Task DeleteAccess(long accessId);
        Task UnDeleteAccess(long accessId);
    }
}
