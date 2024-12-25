using Hot4.Core.Helper;
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
        public async Task<List<AccountSearchModel>> SearchAccount(string filter, int pageNumber, int pageSize)
        {
            var accDetail = _commonRepository.GetViewAccount();
            var result = (from vwa in accDetail
                          join account in
                              (from tblAccount in _context.Account
                               where (tblAccount.AccountName + tblAccount.ReferredBy + tblAccount.Email).Contains(filter)
                               select tblAccount.AccountId
                              ).Union(
                              from tblAccess in _context.Access
                              where tblAccess.AccessCode.Contains(filter)
                              select tblAccess.AccountId
                              )
                          on vwa.AccountId equals account
                          orderby vwa.Balance descending
                          select new AccountSearchModel
                          {
                              AccountId = vwa.AccountId,
                              AccountName = vwa.AccountName,
                              Email = vwa.EmailId,
                              NationalId = vwa.NationalId,
                              ProfileName = vwa.ProfileName,
                              ReferredBy = vwa.RefferedBy,
                              ProfileId = vwa.ProfileId,
                              Balance = vwa.Balance,
                              SaleValue = vwa.SaleValue,
                              USDBalance = vwa.USDBalance,
                              USDUtilityBalance = vwa.USDUtilityBalance,
                              ZESABalance = vwa.ZESABalance
                          });

            return await QuerableFilter.GetPagedData(result, pageNumber, pageSize).Queryable.ToListAsync();
        }
        public async Task<ViewAccountModel?> AccountSelect(long accountId)
        {
            var result = _commonRepository.GetViewAccount();
            return await result.FirstOrDefaultAsync(x => x.AccountId == accountId);
        }

        public async Task DeleteAccount(Account account)
        {
            await Delete(account);
            await SaveChanges();
        }
    }
}
