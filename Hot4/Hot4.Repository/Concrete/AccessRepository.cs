using Hot4.Core.DataViewModels;
using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class AccessRepository : RepositoryBase<Access>, IAccessRepository
    {
        public AccessRepository(HotDbContext context) : base(context) { }
        public async Task<Access?> GetAccess(long accessId)
        {
            return await GetById(accessId);
        }
        public async Task<List<Access>> ListAccountChannel(long accountId, byte channelId)
        {
            return await GetByCondition(d => d.AccountId == accountId && d.ChannelId == channelId).OrderByDescending(d => d.AccessId).ToListAsync();
        }
        public async Task<AccountAccessModel?> GetByAccessCode(string accessCode)
        {
            AccountAccessModel? responseData = null;

            var access = await _context.Access.Include(d => d.Channel).FirstOrDefaultAsync(d => d.AccessCode == accessCode);
            if (access != null)
            {
                responseData = new AccountAccessModel
                {
                    AccessID = access.AccessId,
                    AccountID = access.AccountId,
                    ChannelID = access.ChannelId,
                    Channel = access.Channel.Channel,
                    AccessCode = access.AccessCode,
                    AccessPassword = "********",
                    Deleted = access.Deleted ?? false,
                    PasswordHash = "********",
                    PasswordSalt = access.PasswordSalt
                };
            }
            return responseData;
        }
        public async Task AddAccess(Access access)
        {
            access.AccessCode = access.AccessCode.Replace(" ", "");
            access.Deleted = false;
            access.PasswordSalt = Helper.GenerateSalt(access.AccountId);
            access.PasswordHash = Helper.GeneratePasswordHash(access.PasswordSalt, access.AccessPassword);
            access.InsertDate = DateTime.Now;
            await Create(access);
            await SaveChanges();
        }
        public async Task UpdateAccess(Access access)
        {
            var accessRecord = await GetById(access.AccessId);
            if (accessRecord != null)
            {
                access.Deleted = false;
                access.PasswordSalt = string.IsNullOrEmpty(access.PasswordSalt) ? Helper.GenerateSalt(access.AccountId) : access.PasswordSalt;
                access.PasswordHash = Helper.GeneratePasswordHash(access.PasswordSalt, access.AccessPassword);
                await Update(access);
                await SaveChanges();
            }
        }
        public async Task<List<AccountAccessModel>> ListAccess(long accountId, bool isGetAll, bool isDeleted)
        {
            var query = _context.Access.Include(d => d.Channel).Where(d => d.AccountId == accountId);

            if (!isGetAll)
            {
                query = query.Where(d => d.Deleted == isDeleted);
            }

            var accessList = await query
        .Select(d => new AccountAccessModel
        {
            AccessID = d.AccessId,
            AccountID = d.AccountId,
            ChannelID = d.ChannelId,
            Channel = d.Channel.Channel,
            AccessCode = d.AccessCode,
            AccessPassword = "********",
            Deleted = d.Deleted ?? false,
            PasswordHash = "********",
            PasswordSalt = d.PasswordSalt
        })
        .OrderBy(d => new { d.Channel, d.AccessCode })
        .ToListAsync();
            return accessList;

        }
        public async Task<long> GetAdminId(long accountId)
        {
            var emailAdmin = await GetByCondition(d => d.AccountId == accountId && d.ChannelId == (int)ChannelName.Web).Select(d => (long?)d.AccessId).MinAsync();
            var mobileAdmin = await GetByCondition(d => d.AccountId == accountId).Select(d => (long?)d.AccessId).MinAsync();
            return emailAdmin ?? mobileAdmin ?? 0;
        }
        public async Task PasswordChange(long accessId, string newPassword)
        {
            var access = await GetById(accessId);
            if (access != null)
            {
                string salt = string.IsNullOrEmpty(access.PasswordSalt) ? Helper.GenerateSalt(accessId) : access.PasswordSalt;
                string passwordHash = Helper.GeneratePasswordHash(salt, newPassword);
                access.AccessPassword = newPassword;
                access.PasswordHash = passwordHash;
                access.PasswordSalt = access.PasswordSalt ?? salt;
                await Update(access);
                await SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Access record not found.");
            }
        }
        public async Task<AccountAccessModel?> GetLoginDetails(string accessCode, string accessPassword)
        {
            AccountAccessModel? responseData = null;

            string hashedPassword = Helper.GetMd5Hash(accessPassword);
            var access = await _context.Access.Include(d => d.Channel).FirstOrDefaultAsync(d => d.AccessCode == accessCode && d.Deleted == false
&& d.AccessPassword == accessPassword && d.PasswordHash == hashedPassword);
            if (access != null)
            {
                responseData = new AccountAccessModel
                {
                    AccessID = access.AccessId,
                    AccountID = access.AccountId,
                    ChannelID = access.ChannelId,
                    Channel = access.Channel.Channel,
                    AccessCode = access.AccessCode,
                    AccessPassword = "********",
                    Deleted = access.Deleted ?? false,
                    PasswordHash = "********",
                    PasswordSalt = access.PasswordSalt
                };

            }
            return responseData;
        }
        public async Task DeleteAccess(long accessId)
        {
            var access = await GetById(accessId);
            if (access != null)
            {
                access.Deleted = true;
                await Update(access);
                await SaveChanges();
            }
        }
        public async Task UnDeleteAccess(long accessId)
        {
            var access = await GetById(accessId);
            if (access != null)
            {
                access.Deleted = false;
                await Update(access);
                await SaveChanges();
            }
        }

    }
}
