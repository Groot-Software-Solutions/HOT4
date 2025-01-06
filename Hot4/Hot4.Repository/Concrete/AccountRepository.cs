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
        public async Task<long> AddAccount(Account account)
        {
            account.AccountName = account.AccountName.Replace("\n", "|").Replace("\r", "|");
            account.InsertDate = DateTime.Now;
            await Create(account);
            await SaveChanges();
            return account.AccountId;
        }
        public async Task UpdateAccount(Account account)
        {
            var accountRecord = await GetById(account.AccountId);
            if (accountRecord != null)
            {
                Update(account);
                await SaveChanges();
            }
        }
        public async Task<AccountModel?> GetAccountById(long accountId)
        {
            var result = await GetById(accountId);
            if (result != null)
            {
                return new AccountModel
                {
                    AccountName = result.AccountName,
                    AccountId = result.AccountId,
                    Email = result.Email,
                    InsertDate = result.InsertDate,
                    NationalId = result.NationalId,
                    ReferredBy = result.ReferredBy,
                    ProfileId = result.ProfileId,
                };
            }
            return null;
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

            var result = await _commonRepository.GetViewAccountList(accountIds);

            return result.Skip((pageNo - 1) * pageSize)
                         .Take(pageSize).ToList();
        }
        public async Task<ViewAccountModel?> GetAccountDetailById(long accountId)
        {
            var accIds = new List<long>()
           {
               accountId
           };
            var result = await _commonRepository.GetViewAccountList(accIds);
            return result.FirstOrDefault();
        }

        public async Task DeleteAccount(Account account)
        {
            Delete(account);
            await SaveChanges();
        }
    }
}
