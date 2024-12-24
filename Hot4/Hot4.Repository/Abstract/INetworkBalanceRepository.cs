using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface INetworkBalanceRepository
    {
        Task<NetworkBalanceModel> GetNetworkBalance(int BrandId);
        Task<List<NetworkBalanceModel>> ListNetworkBalance(int BrandId);
    }
}
