using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task<List<Banks>> ListBanks();
        Task AddBank(Banks  banks);
        Task UpdateBank(Banks banks);
        Task DeleteBank(Banks banks);
    }
}
