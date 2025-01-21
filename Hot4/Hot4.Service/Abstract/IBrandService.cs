using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBrandService
    {
        Task<BrandModel?> GetBrandById(byte BrandId);
        Task<List<BrandModel>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel);
        Task<List<BrandModel>> ListBrand();
        Task<bool> AddBrand(BrandRecord brand);
        Task<bool> UpdateBrand(BrandRecord brand);
        Task<bool> DeleteBrand(byte brandId);
    }
}
