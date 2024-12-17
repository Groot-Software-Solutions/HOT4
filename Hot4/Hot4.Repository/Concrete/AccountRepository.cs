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
            await Update(account);
            await SaveChanges();
        }
        public async Task<Account?> GetAccount(long accountId)
        {
            return await GetById(accountId);
        }
        //public async Task<AccountBalanceModel?> GetAccountWithBalances(long accountId)
        //{
        //    var result = await (from vwbal in _context.VwBalance.Where(d => d.AccountId == accountId)
        //                        join acss in _context.Access
        //                        on vwbal.AccountId equals acss.AccountId

        //                        select new AccountBalanceModel
        //                        {
        //                            AccountID = vwbal.AccountId,
        //                            Balance = vwbal.Balance,
        //                            SaleValue = vwbal.SaleValue,
        //                            USDBalance = vwbal.Usdbalance,
        //                            USDUtilityBalance = vwbal.UsdutilityBalance,
        //                            ZESABalance = vwbal.Zesabalance
        //                        })
        //                          .OrderByDescending(d => d.Balance).FirstOrDefaultAsync();


        //    return result;
        //}
        public async Task<List<AccountSearchModel>> SearchAccount(string filter, int pageNumber, int pageSize)
        {

            //var res = (from acc in _context.VwAccount
            //           join acss in _context.Access
            //               on acc.AccountId equals acss.AccountId
            //           where (EF.Constant(acc.AccountName).Contains(filter, StringComparison.OrdinalIgnoreCase)
            //                  || EF.Constant(acc.Email).Contains(filter, StringComparison.OrdinalIgnoreCase)
            //                  || EF.Constant(acc.ReferredBy).Contains(filter, StringComparison.OrdinalIgnoreCase)
            //                  || EF.Constant(acss.AccessCode).Contains(filter)
            //                  || acc.AccountId.ToString() == filter)
            //           select new AccountSearchModel
            //           {
            //               AccountID = acc.AccountId,
            //               AccountName = acc.AccountName,
            //               Email = acc.Email,
            //               NationalID = acc.NationalId,
            //               ProfileName = acc.ProfileName,
            //               ReferredBy = acc.ReferredBy
            //           });

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

            // OR
            //var accountIdsFromTblAccount = from a in _context.Account
            //                               where (a.AccountName + a.ReferredBy + a.Email).Contains(filter)
            //                               select a.AccountId;

            //var accountIdsFromTblAccess = from a in _context.Access
            //                              where a.AccessCode.Contains(filter)
            //                              select a.AccountId;


            //var combinedAccountIds = accountIdsFromTblAccount
            //    .Union(accountIdsFromTblAccess)
            //    .Distinct();

            //var result2 = from vwa in _context.VwAccount
            //              join aId in combinedAccountIds on vwa.AccountId equals aId
            //              orderby vwa.Balance descending
            //              select new AccountSearchModel
            //              {
            //                  AccountID = vwa.AccountId,
            //                  AccountName = vwa.AccountName,
            //                  Email = vwa.Email,
            //                  NationalID = vwa.NationalId,
            //                  ProfileName = vwa.ProfileName,
            //                  ReferredBy = vwa.ReferredBy,
            //                  ProfileID = vwa.ProfileId,
            //                  Balance = vwa.Balance,
            //                  SaleValue = vwa.SaleValue,
            //                  USDBalance = vwa.Usdbalance,
            //                  USDUtilityBalance = vwa.UsdutilityBalance,
            //                  ZESABalance = vwa.Zesabalance
            //              };

            return await QuerableFilter.GetPagedData(result, pageNumber, pageSize).Queryable.ToListAsync();
        }
        public async Task<VwAccount?> AccountSelect(long accountId)
        {
            return await _context.VwAccount.FirstOrDefaultAsync(d => d.AccountId == accountId);
        }
        //public async Task<Access> AddWithAccessWithTransaction(Account account, Access access)
        //{
        //    try
        //    {
        //        await using var transaction = await _context.Database.BeginTransactionAsync();
        //        await _context.Account.AddAsync(account);

        //        access.AccountId = account.AccountId;
        //        await _context.Access.AddAsync(access);
        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //    }
        //    catch (Exception ex)
        //    {
        //        if (_context.Database.CurrentTransaction != null)
        //        {
        //            await _context.Database.RollbackTransactionAsync();
        //        }
        //        throw new InvalidOperationException("An error occurred while processing the transaction.", ex);
        //    }
        //    return access;

        //}

    }
}
