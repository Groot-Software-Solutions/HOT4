using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBundleRepository
    {
        Task<List<BundleModel>> GetBundles(int bundleId);
        Task<List<BundleModel>> ListBundles();
    }
}
