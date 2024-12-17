
using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAccountRepository
    {
        Task<long> AddAccount(Account account);
        Task UpdateAccount(Account account);
        Task<Account?> GetAccount(long accountId);
        //  Task<AccountBalanceModel?> GetAccountWithBalances(long accountId);
        Task<List<AccountSearchModel>> SearchAccount(string filter, int pageNumber, int pageSize);
        Task<VwAccount?> AccountSelect(long accountId);
        //  Task<Access> AddWithAccessWithTransaction(Account account, Access access);
    }
}
