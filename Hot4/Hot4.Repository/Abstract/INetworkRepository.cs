using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface INetworkRepository
    {
        Task AddNetwork(Networks networks);
        Task UpdateNetwork(Networks networks);
        Task DeleteNetwork(Networks networks);
        Task<List<NetworkModel>> GetNetworkIdentityByMobile(string mobile);
    }
}
