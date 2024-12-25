using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxStateRepository
    {
        Task<List<BankTransactionStateModel>> ListBankTrxStates();
        Task AddBankTrxState(BankTrxStates bankState);
        Task UpdateBankTrxState(BankTrxStates bankState);
        Task DeleteBankTrxState(BankTrxStates bankState);
    }
}
