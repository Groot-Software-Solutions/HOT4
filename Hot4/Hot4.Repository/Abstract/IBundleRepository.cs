using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBundleRepository
    {
        Task<List<BundleModel>> GetBundles(int bundleId);
        Task<List<BundleModel>> ListBundles();
        Task AddBundle(Bundle bundle);
        Task UpdateBundle(Bundle bundle);
        Task DeleteBundle(Bundle bundle);
    }
}
