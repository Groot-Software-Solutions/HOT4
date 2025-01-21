using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        private ICommonRepository _commonRepository;
        public AccountRepository(HotDbContext context, ICommonRepository commonRepository) : base(context)
        {
            _commonRepository = commonRepository;
        }
        public async Task<bool> AddAccount(Account account)
        {
            account.AccountName = account.AccountName.Replace("\n", "|").Replace("\r", "|");
            account.InsertDate = DateTime.Now;
            await Create(account);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateAccount(Account account)
        {
            Update(account);
            await SaveChanges();
            return true;
        }
        public async Task<Account?> GetAccountById(long accountId)
        {
            return await GetById(accountId);
        }
        public async Task<List<ViewAccountModel>> SearchAccount(string filter, int pageNo, int pageSize)
        {
            var accountIds = await (from a in _context.Account
                                    where (a.AccountName + a.ReferredBy + a.Email).Contains(filter)
                                    select a.AccountId)
                                          .Union(
                        from ac in _context.Access
                        where ac.AccessCode.Contains(filter)
                        select ac.AccountId
                   )
                   .ToListAsync();

            return await _commonRepository.GetViewAccountList(accountIds, pageNo, pageSize);

        }
        public async Task<ViewAccountModel?> GetAccountDetailById(long accountId)
        {
            var result = await _commonRepository.GetViewAccountList(new List<long>() { accountId }, 1, 10);
            return result.FirstOrDefault();
        }
        public async Task<bool> DeleteAccount(Account account)
        {
            Delete(account);
            await SaveChanges();
            return true;
        }
    }
}
