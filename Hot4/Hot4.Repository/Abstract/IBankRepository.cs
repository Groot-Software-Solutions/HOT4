using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task<List<BankModel>> ListBanks();
        Task AddBank(Banks bank);
        Task UpdateBank(Banks bank);
        Task DeleteBank(Banks bank);
    }
}
