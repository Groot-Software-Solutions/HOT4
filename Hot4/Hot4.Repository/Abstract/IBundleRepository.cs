using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBundleRepository
    {
        Task<BundleModel?> GetBundlesById(int bundleId);
        Task<List<BundleModel>> ListBundles();
        Task<bool> AddBundle(Bundle bundle);
        Task<bool> UpdateBundle(Bundle bundle);
        Task<bool> DeleteBundle(Bundle bundle);
    }
}
