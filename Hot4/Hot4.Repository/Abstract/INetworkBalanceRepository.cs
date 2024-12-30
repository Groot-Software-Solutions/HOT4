using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface INetworkBalanceRepository
    {
        Task<List<NetworkBalanceModel>> ListNetworkBalance(int BrandId);
    }
}
