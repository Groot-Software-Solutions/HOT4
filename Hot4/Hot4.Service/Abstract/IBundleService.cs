using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBundleService
    {
        Task<BundleModel?> GetBundlesById(int bundleId);
        Task<List<BundleModel>> ListBundles();
        Task<bool> AddBundle(BundleToDo bundleModel);
        Task<bool> UpdateBundle(BundleToDo bundleModel);
        Task<bool> DeleteBundle(int bundleId);
    }
}
