using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface INetworkBalanceRepository
    {
        Task<List<NetworkBalanceModel>> ListNetworkBalance(int BrandId);
    }
}
