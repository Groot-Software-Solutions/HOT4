using Hot4.Core.Enums;
using Hot4.Core.Helper;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class AccessRepository : RepositoryBase<Access>, IAccessRepository
    {
        public AccessRepository(HotDbContext context) : base(context) { }
        public async Task<Access?> GetAccessById(long accessId)
        {
            var record = await GetById(accessId);
            return record;
        }
        public async Task<List<Access>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId)
        {
            return  await GetByCondition(d => d.AccountId == accountId
                         && d.ChannelId == channelId)
                         .OrderByDescending(d => d.AccessId).ToListAsync();    
        }
        public async Task<Access?> GetAccessByCode(string accessCode)
        {
            var record = await _context.Access.Include(d => d.Channel).FirstOrDefaultAsync(d => d.AccessCode == accessCode);
            return record;
        }
        public async Task<bool> AddAccess(Access access)
        {
            access.AccessCode = access.AccessCode.Replace(" ", "");
            access.Deleted = false;
            access.PasswordSalt = Helper.GenerateSalt(access.AccountId);
            access.PasswordHash = Helper.GeneratePasswordHash(access.PasswordSalt, access.AccessPassword);
            access.InsertDate = DateTime.Now;
            await Create(access);
            await SaveChanges();
            return true;
        }
        public async Task<bool> AddAccessDeprecated(Access access)
        {
            access.InsertDate = DateTime.Now;
            access.Deleted = false;
            access.AccessPassword = "DEPRECATED";
            await Create(access);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateAccess(Access access)
        {            
            access.Deleted = false;
            access.PasswordSalt = string.IsNullOrEmpty(access.PasswordSalt) ? Helper.GenerateSalt(access.AccountId) : access.PasswordSalt;
            access.PasswordHash = Helper.GeneratePasswordHash(access.PasswordSalt, access.AccessPassword);
            Update(access);
            await SaveChanges();
            return true;
        }      
        public async Task<List<Access>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted)
        {
            var records = await GetByCondition(d => d.AccountId == accountId)
                              .Include(d => d.Channel)
                              .OrderBy(d => d.Channel)
                                .ThenBy(d => d.AccessCode)
                              .ToListAsync();

            if (!isGetAll)
            {
                records = records.Where(d => d.Deleted == isDeleted).ToList();
            }

            return records;

        }
        public async Task<long> GetAdminId(long accountId)
        {
            var emailAdmin = await GetByCondition(d => d.AccountId == accountId && d.ChannelId == (int)ChannelName.Web).Select(d => (long?)d.AccessId).MinAsync();
            var mobileAdmin = await GetByCondition(d => d.AccountId == accountId).Select(d => (long?)d.AccessId).MinAsync();
            return emailAdmin ?? mobileAdmin ?? 0;
        }   
        public async Task<bool> PasswordChange(Access access,long accessId , string newPassword)
        {
                string salt = Helper.GenerateSalt(accessId);
                string passwordHash = Helper.GeneratePasswordHash(access.PasswordSalt ?? salt, newPassword);
                access.AccessPassword = newPassword;
                access.PasswordHash = passwordHash;
                access.PasswordSalt = access.PasswordSalt ?? salt;
                Update(access);
                await SaveChanges();
            return true;
            
        }
        public async Task<bool> PasswordChangeDeprecated(Access access,string passwordHash, string passwordSalt)
        {
                access.PasswordHash = passwordHash;
                access.PasswordSalt = passwordSalt;
                access.AccessPassword = "DEPRECATED";
                Update(access);
                await SaveChanges();
            return true;
            
        }
        public async Task<Access?> GetLoginDetails(string accessCode, string accessPassword)
        {
            var record = await _context.Access.Include(d => d.Channel)
                .FirstOrDefaultAsync(d => d.AccessCode == accessCode && d.Deleted == false);
            if (record == null)
            {
                return null;
            }
            string passwordSalt = record.PasswordSalt;
            string hashedPassword = Helper.GeneratePasswordHash(passwordSalt, accessPassword);

            if (record.AccessPassword == accessCode || record.PasswordHash == hashedPassword)
            {
                return record;
            }
            return null;
        }
        public async Task<Access?> GetLoginDetailsByAccessCode(string accessCode)
        {
            var record = await _context.Access
                         .Include(d => d.Channel)
                         .FirstOrDefaultAsync(d => d.AccessCode == accessCode && d.Deleted == false);

            if (record != null)
            {
                return record;
            }
            return null;
        }
        public async Task<bool> DeleteAccess(Access access)
        {            
                access.Deleted = true;
                Update(access);
                await SaveChanges();
                return true;            
        }
        public async Task<bool> UnDeleteAccess(Access access)
        {          
                access.Deleted = false;
                Update(access);
                await SaveChanges();
            return true;
            
        }

    }
}
