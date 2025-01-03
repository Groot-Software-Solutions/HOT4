using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IBrandRepository
    {
        Task<BrandModel> GetBrandById(int BrandId);
        Task<List<BrandModel>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel);
        Task<List<BrandModel>> ListBrand();
        Task AddBrand(Brand brand);
        Task UpdateBrand(Brand brand);
        Task DeleteBrand(Brand brand);
    }
}
