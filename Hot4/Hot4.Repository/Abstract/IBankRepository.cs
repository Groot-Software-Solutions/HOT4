using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBankRepository
    {
        Task<Banks> GetByBankId(byte BankId);
        Task<List<Banks>> ListBanks();
        Task<bool> AddBank(Banks  banks);
        Task<bool> UpdateBank(Banks banks);
        Task<bool> DeleteBank(Banks banks);
    }
}
