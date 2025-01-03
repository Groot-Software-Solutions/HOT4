using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IWalletTypeRepository
    {
        Task AddWalletType(WalletType walletType);
        Task UpdateWalletType(WalletType walletType);
        Task DeleteWalletType(WalletType walletType);
        Task<WalletTypeModel?> GetWalletTypeById(int walletTypeId);
        Task<List<WalletTypeModel>> ListWalletType();
    }
}
