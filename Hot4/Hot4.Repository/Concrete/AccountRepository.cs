using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
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
            await Create(account);
            await SaveChanges();
            return account.AccountId;
        }
        public async Task UpdateAccount(Account account)
        {
            var accountRecord = await GetById(account.AccountId);
            if (accountRecord != null)
            {
                await Update(account);
                await SaveChanges();
            }
        }
        public async Task<AccountModel?> GetAccount(long accountId)
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
            else
            {
                return null;
            }
        }
        public async Task<List<ViewAccountModel>> SearchAccount(string filter, int pageNo, int pageSize)
        {
            var filteredAccounts = await (from a in _context.Account
                                          where (a.AccountName + a.ReferredBy + a.Email).Contains(filter)
                                          select a.AccountId).ToListAsync();

            var filteredAccess = await (from ac in _context.Access
                                        where ac.AccessCode.Contains(filter)
                                        select ac.AccountId).ToListAsync();

            var combinedAccountIds = filteredAccounts.Concat(filteredAccess);

            var result = await _commonRepository.GetViewAccountList(combinedAccountIds.ToList());

            return result.Skip((pageNo - 1) * pageSize)
                         .Take(pageSize).ToList();
        }
        public async Task<ViewAccountModel?> AccountSelect(long accountId)
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
            await Delete(account);
            await SaveChanges();
        }
    }
}
