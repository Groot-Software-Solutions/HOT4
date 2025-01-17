using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxStateRepository
    {
        Task<List<BankTrxStates>> ListBankTrxStates();
        Task<BankTrxStates?> GetBankTrxStateById(byte bankTrxStateId);
        Task<bool> AddBankTrxState(BankTrxStates bankState);
        Task<bool> UpdateBankTrxState(BankTrxStates bankState);
        Task<bool> DeleteBankTrxState(BankTrxStates bankState);
    }
}
