using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IWalletTypeRepository
    {
        Task<bool> AddWalletType(WalletType walletType);
        Task<bool> UpdateWalletType(WalletType walletType);
        Task<bool> DeleteWalletType(WalletType walletType);
        Task<WalletType?> GetWalletTypeById(int walletTypeId);
        Task<List<WalletType>> ListWalletType();
    }
}
