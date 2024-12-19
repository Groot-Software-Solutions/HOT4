using Hot4.Core.DataViewModels;
using Hot4.Core.Helper;
using Hot4.DataModel.Data;

using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HotDbContext context) : base(context) { }
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
        public async Task<Account?> GetAccount(long accountId)
        {
            return await GetById(accountId);
        }
        public async Task<List<AccountSearchModel>> SearchAccount(string filter, int pageNumber, int pageSize)
        {
            var result = (from vwa in _context.VwAccount
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
                              AccountID = vwa.AccountId,
                              AccountName = vwa.AccountName,
                              Email = vwa.Email,
                              NationalID = vwa.NationalId,
                              ProfileName = vwa.ProfileName,
                              ReferredBy = vwa.ReferredBy,
                              ProfileID = vwa.ProfileId,
                              Balance = vwa.Balance,
                              SaleValue = vwa.SaleValue,
                              USDBalance = vwa.Usdbalance,
                              USDUtilityBalance = vwa.UsdutilityBalance,
                              ZESABalance = vwa.Zesabalance
                          });

            return await QuerableFilter.GetPagedData(result, pageNumber, pageSize).Queryable.ToListAsync();
        }
        public async Task<VwAccount?> AccountSelect(long accountId)
        {
            return await _context.VwAccount.FirstOrDefaultAsync(d => d.AccountId == accountId);
        }
    }
}
