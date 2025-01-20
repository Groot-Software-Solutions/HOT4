using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IAccountService
    {
        Task<bool> AddAccount(AccountModel account);
        Task<bool> UpdateAccount(AccountModel account);
        Task<AccountModel?> GetAccountById(long accountId);
        Task<List<ViewAccountModel>> SearchAccount(string filter, int pageNo, int pageSize);
        Task<ViewAccountModel?> GetAccountDetailById(long accountId);
        Task<bool> DeleteAccount(long accountId);
    }
}
