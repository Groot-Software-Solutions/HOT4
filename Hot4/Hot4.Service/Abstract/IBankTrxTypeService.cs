using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankTrxTypeService
    {
        Task<List<BankTransactionTypeModel>> ListBankTrxType();
        Task<BankTransactionTypeModel?> GetBankTrxTypeById(byte bankTrxTypeId);
        Task<bool> AddBankTrxType(BankTransactionTypeModel bankType);
        Task<bool> UpdateBankTrxType(BankTransactionTypeModel bankType);
        Task<bool> DeleteBankTrxType(byte bankTrxTypeId);
    }
}
