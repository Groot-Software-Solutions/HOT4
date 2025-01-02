using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccountRepository
    {
        Task<long> AddAccount(Account account);
        Task UpdateAccount(Account account);
        Task<AccountModel?> GetAccountById(long accountId);
        Task<List<ViewAccountModel>> SearchAccount(string filter, int pageNumber, int pageSize);
        Task<ViewAccountModel?> GetAccountDetailById(long accountId);
        Task DeleteAccount(Account account);
    }
}
