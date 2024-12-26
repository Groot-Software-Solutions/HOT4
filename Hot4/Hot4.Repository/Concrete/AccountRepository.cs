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

            //var accDetail = _commonRepository.GetViewAccount();
            //var result = (from vwa in accDetail
            //              join account in
            //                  (from tblAccount in _context.Account
            //                   where (tblAccount.AccountName + tblAccount.ReferredBy + tblAccount.Email).Contains(filter)
            //                   select tblAccount.AccountId
            //                  ).Union(
            //                  from tblAccess in _context.Access
            //                  where tblAccess.AccessCode.Contains(filter)
            //                  select tblAccess.AccountId
            //                  )
            //              on vwa.AccountId equals account
            //              orderby vwa.Balance descending
            //              select new AccountSearchModel
            //              {
            //                  AccountId = vwa.AccountId,
            //                  AccountName = vwa.AccountName,
            //                  Email = vwa.EmailId,
            //                  NationalId = vwa.NationalId,
            //                  ProfileName = vwa.ProfileName,
            //                  ReferredBy = vwa.RefferedBy,
            //                  ProfileId = vwa.ProfileId,
            //                  Balance = vwa.Balance,
            //                  SaleValue = vwa.SaleValue,
            //                  USDBalance = vwa.USDBalance,
            //                  USDUtilityBalance = vwa.USDUtilityBalance,
            //                  ZESABalance = vwa.ZESABalance
            //              });

            //return await PaginationFilter.GetPagedData(result, pageNumber, pageSize).Queryable.ToListAsync();
            try
            {
                var filteredAccounts = await (from a in _context.Account
                                                  //    where (a.AccountName.Contains(filter) 
                                              where (a.AccountName + a.ReferredBy + a.Email).Contains(filter)
                                              //|| a.ReferredBy.Contains(filter)
                                              //    || a.Email.Contains(filter)

                                              select a.AccountId).ToListAsync();

                var filteredAccess = await (from ac in _context.Access
                                            where ac.AccessCode.Contains(filter)
                                            select ac.AccountId).ToListAsync();

                var combinedAccountIds = filteredAccounts.Concat(filteredAccess);

                var result = await _commonRepository.GetViewAccountList(combinedAccountIds.ToList());

                return result.Skip((pageNo - 1) * pageSize)
                             .Take(pageSize).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public async Task<ViewAccountModel?> AccountSelect(long accountId, int pageNumber, int pageSize)
        {
            //  var result = _commonRepository.GetViewAccount();
            // return await result.FirstOrDefaultAsync(x => x.AccountId == accountId);
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
