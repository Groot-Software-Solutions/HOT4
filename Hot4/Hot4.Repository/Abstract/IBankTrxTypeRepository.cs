using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxTypeRepository
    {
        Task<List<BankTransactionTypeModel>> ListBankTrxType();
    }
}
