using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxStateRepository
    {
        Task<List<BankTransactionStateModel>> ListBankTrxStates();
    }
}
