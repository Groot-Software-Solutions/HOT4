using Hot4.Core.DataViewModels;
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

        public async Task<Access?> GetByAccessCode(string accessCode)
        {
            return await GetByCondition(d => d.AccessCode == accessCode).FirstOrDefaultAsync();
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

            if (isGetAll)
            {
                return await _context.Access.Include(d => d.Channel)
                .Where(d => d.AccountId == accountId)
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
                }).OrderBy(d => new { d.Channel, d.AccessCode }).ToListAsync();
            }
            else
            {
                if (isDeleted)
                {
                    return await _context.Access.Include(d => d.Channel)
               .Where(d => d.AccountId == accountId && d.Deleted == true)
               .Select(d => new AccountAccessModel
               {
                   AccessID = d.AccessId,
                   AccountID = d.AccountId,
                   ChannelID = d.ChannelId,
                   Channel = d.Channel.Channel,
                   AccessCode = d.AccessCode,
                   AccessPassword = "********",
                   Deleted = d.Deleted ?? false
               }).OrderBy(d => new { d.Channel, d.AccessCode }).ToListAsync();
                }
                else
                {
                    return await _context.Access.Include(d => d.Channel)
                   .Where(d => d.AccountId == accountId && d.Deleted == false)
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

                   }).OrderBy(d => new { d.Channel, d.AccessID, d.AccessCode }).ToListAsync();
                }
            }
        }


        public async Task<long> GetAdminIDAsync(long accountId)
        {
            var emailAdmin = await GetByCondition(d => d.AccountId == accountId && d.ChannelId == 2).Select(d => (long?)d.AccessId).MinAsync();

            var mobileAdmin = await GetByCondition(d => d.AccountId == accountId).Select(d => (long?)d.AccessId).MinAsync();


            return emailAdmin ?? mobileAdmin ?? 0;
        }

        public bool SMSLoginIsValid(Access access, string password)
        {
            if (access.PasswordHash is not null)
            {
                var hashedPassword = Helper.ToMD5Hash($"{access.PasswordSalt}{password}").ToLower();
                return hashedPassword == access.PasswordHash.ToLower();
            }
            return access.AccessPassword == password;
        }
    }
}
