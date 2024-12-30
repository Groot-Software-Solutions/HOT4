using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface INetworkRepository
    {
        Task<List<NetworkModel>> GetNetworkIdentity(string mobile);
    }
}
