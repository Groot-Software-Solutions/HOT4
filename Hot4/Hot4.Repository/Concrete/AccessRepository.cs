using Hot4.Core.DataViewModels;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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
            return await GetByCondition(d => d.AccessCode == accessCode)
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
                }).FirstOrDefaultAsync();
        }
        public async Task AddAccess(Access access)
        {
            await Create(access);
            await SaveChanges();

        }
        public async Task UpdateAccess(Access access)
        {
            await Update(access);
            await SaveChanges();
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
        public async Task<long> GetAdminID(long accountId)
        {
            var emailAdmin = await GetByCondition(d => d.AccountId == accountId && d.ChannelId == 2).Select(d => (long?)d.AccessId).MinAsync();
            var mobileAdmin = await GetByCondition(d => d.AccountId == accountId).Select(d => (long?)d.AccessId).MinAsync();
            return emailAdmin ?? mobileAdmin ?? 0;
        }

        public async Task PasswordChange(long accessId, string newPassword)
        {
            var access = await GetById(accessId);
            if (access != null)
            {

                string salt = GenerateSalt(accessId);
                string passwordHash = GeneratePasswordHash(salt, newPassword);

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
        public async Task<AccountAccessModel?> LoginSelect(string accessCode, string accessPassword)
        {
            string hashedPassword = GetMd5Hash(accessPassword);

            return await _context.Access.Include(d => d.Channel)
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
                .FirstOrDefaultAsync(d => d.AccessCode == accessCode && d.Deleted == false
            && d.AccessPassword == accessPassword && d.PasswordHash == hashedPassword);
        }
        private string GenerateSalt(long accessID)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(accessID.ToString()));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower().Substring(0, 20);
            }
        }

        private string GeneratePasswordHash(string salt, string newPassword)
        {
            string combined = (salt ?? "") + newPassword;
            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        private string GetMd5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var byteValue in hashBytes)
                {
                    sb.Append(byteValue.ToString("x2"));
                }
                return sb.ToString().ToLower(); // Return in lowercase format
            }
        }
    }
}
