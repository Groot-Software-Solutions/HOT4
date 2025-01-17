using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBrandRepository
    {
        Task<Brand?> GetBrandById(byte brandId);
        Task<List<Brand>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel);
        Task<List<Brand>> ListBrand();
        Task<bool> AddBrand(Brand brand);
        Task<bool> UpdateBrand(Brand brand);
        Task<bool> DeleteBrand(Brand brand);
    }
}
