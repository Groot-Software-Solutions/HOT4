using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IBrandRepository
    {
        Task<List<BrandModel>> GetBrand(int BrandId);
        Task<List<BrandModel>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel);
        Task<List<BrandModel>> ListBrand();
    }
}
