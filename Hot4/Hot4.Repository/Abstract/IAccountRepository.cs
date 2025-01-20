using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IAccountRepository
    {
        Task<bool> AddAccount(Account account);
        Task<bool> UpdateAccount(Account account);
        Task<Account?> GetAccountById(long accountId);
        Task<List<ViewAccountModel>> SearchAccount(string filter, int pageNo, int pageSize);
        Task<ViewAccountModel?> GetAccountDetailById(long accountId);
        Task<bool> DeleteAccount(Account account);
    }
}
