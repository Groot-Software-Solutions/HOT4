using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface INetworkBalanceRepository
    {
        public Task<NetworkBalanceModel> GetNetworkBalance(int BrandId);
        public Task<List<NetworkBalanceModel>> ListNetworkBalance(int BrandId);
    }
}
