using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IWalletTypeService
    {
        Task<bool> AddWalletType(WalletTypeModel walletType);
        Task<bool> UpdateWalletType(WalletTypeModel walletType);
        Task<bool> DeleteWalletType(int walletTypeId);
        Task<WalletTypeModel?> GetWalletTypeById(int walletTypeId);
        Task<List<WalletTypeModel>> ListWalletType();
    }
}
