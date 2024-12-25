using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface INetworkRepository
    {
        Task<List<NetworkModel>> GetNetworkIdentity(string mobile);
    }
}
