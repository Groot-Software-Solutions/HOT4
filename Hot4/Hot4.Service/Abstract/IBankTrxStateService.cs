using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankTrxStateService
    {
        Task<List<BankTransactionStateModel>> ListBankTrxStates();
        Task<BankTransactionStateModel?> GetBankTrxStateById(byte bankTrxStateId);
        Task<bool> AddBankTrxState(BankTransactionStateModel bankState);
        Task<bool> UpdateBankTrxState(BankTransactionStateModel bankState);
        Task<bool> DeleteBankTrxState(byte bankTrxStateId);
    }
}
