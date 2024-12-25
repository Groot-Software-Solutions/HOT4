using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IAccountRepository
    {
        Task<long> AddAccount(Account account);
        Task UpdateAccount(Account account);
        Task<AccountModel?> GetAccount(long accountId);
        Task<List<AccountSearchModel>> SearchAccount(string filter, int pageNumber, int pageSize);
        Task<ViewAccountModel?> AccountSelect(long accountId);
    }
}
