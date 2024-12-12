
using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IAccountRepository
    {
        Task<long> AddAccount(TblAccount account);
        Task UpdateAccount(TblAccount account);
        Task<TblAccount?> GetAccount(long accountId);
        Task<AccountBalanceModel?> GetAccountWithBalances(long accountId);
        Task<List<AccountSearchModel>> SearchAccount(string filter, int rows);
        Task<VwAccount?> GetAccountView(long accountId);
        Task<Access> AddWithAccessWithTransaction(TblAccount account, Access access);

    }
}
