using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Concrete
{
    public interface INetworkRepository
    {
        Task<List<NetworkModel>> GetNetworkIdentity(string mobile);
    }
}
