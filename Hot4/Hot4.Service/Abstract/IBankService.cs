using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankService
    {
        Task<BankModel> GetByBankId(byte bankId);
        Task<List<BankModel>> ListBanks();
        Task <bool>AddBank(BankModel bankModel);
        Task <bool>UpdateBank(BankModel bankModel);
        Task<bool> DeleteBank(byte bankId);
    }
}
