using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface INetworkRepository
    {
        Task<bool> AddNetwork(Networks networks);
        Task<bool> UpdateNetwork(Networks networks);
        Task<Networks?> GetNetworkById(byte networkId);
        Task<bool> DeleteNetwork(Networks networks);
        Task<List<Networks>> GetNetworkIdentityByMobile(string mobile);
        Task<List<NetworkBalanceModel>> GetBrandNetworkBalance();
    }
}
