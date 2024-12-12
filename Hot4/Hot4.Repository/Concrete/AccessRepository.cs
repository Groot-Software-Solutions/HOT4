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

        public async Task<List<AccountAccessModel>> ListAccess(long accountId)
        {

            return await (from acss in _context.Access
                          join chn in _context.Channel
                              on acss.ChannelId equals chn.ChannelId
                          where acss.AccountId == accountId
                          select new AccountAccessModel
                          {
                              AccessID = acss.AccessId,
                              AccessCode = acss.AccessCode,
                              Channel = chn.Channel,
                              Deleted = acss.Deleted ?? false,
                              AccountID = acss.AccountId,
                          })
                        .ToListAsync();
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
