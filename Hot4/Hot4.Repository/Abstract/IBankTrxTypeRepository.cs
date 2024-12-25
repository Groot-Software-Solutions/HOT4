using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxTypeRepository
    {
        Task<List<BankTransactionTypeModel>> ListBankTrxType();
        Task AddBankTrxType(BankTrxTypes bankType);
        Task UpdateBankTrxType(BankTrxTypes bankType);
        Task DeleteBankTrxType(BankTrxTypes bankType);
    }
}
