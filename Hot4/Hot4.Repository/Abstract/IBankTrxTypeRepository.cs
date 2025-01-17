using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxTypeRepository
    {
        Task<List<BankTrxTypes>> ListBankTrxType();
        Task<BankTrxTypes?> GetBankTrxTypeById(byte bankTrxTypeId);
        Task<bool> AddBankTrxType(BankTrxTypes bankType);
        Task<bool> UpdateBankTrxType(BankTrxTypes bankType);
        Task<bool> DeleteBankTrxType(BankTrxTypes bankType);
    }
}
