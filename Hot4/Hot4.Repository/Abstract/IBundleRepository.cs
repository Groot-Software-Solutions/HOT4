using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBundleRepository
    {
        Task<BundleModel?> GetBundlesById(int bundleId);
        Task<List<BundleModel>> ListBundles();
        Task AddBundle(Bundle bundle);
        Task UpdateBundle(Bundle bundle);
        Task DeleteBundle(Bundle bundle);
    }
}
