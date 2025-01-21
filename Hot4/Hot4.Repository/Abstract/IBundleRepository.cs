using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBundleRepository
    {
        Task<Bundle?> GetBundlesById(int bundleId);
        Task<List<Bundle>> ListBundles();
        Task<bool> AddBundle(Bundle bundle);
        Task<bool> UpdateBundle(Bundle bundle);
        Task<bool> DeleteBundle(Bundle bundle);
    }
}
