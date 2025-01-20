using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface INetworkService
    {
        Task<bool> AddNetwork(NetworkModel networks);
        Task<bool> UpdateNetwork(NetworkModel networks);
        Task<NetworkModel?> GetNetworkById(byte networkId);
        Task<bool> DeleteNetwork(byte networkId);
        Task<List<NetworkModel>> GetNetworkIdentityByMobile(string mobile);
        Task<List<NetworkBalanceModel>> GetBrandNetworkBalance();
    }
}
