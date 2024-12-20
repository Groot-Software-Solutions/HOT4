using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxTypeRepository
    {
        Task<List<BankTrxTypes>> ListBankTrxType();
    }
}
