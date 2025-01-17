using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BrandRepository : RepositoryBase<Brand>, IBrandRepository
    {
        public BrandRepository(HotDbContext context) : base(context) { }

        public async Task<Brand?> GetBrandById(byte BrandId)
        {
            return await _context.Brand.Include(d => d.Network)
                .FirstOrDefaultAsync(d => d.BrandId == BrandId);
        }

        public async Task<List<Brand>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel)
        {
            return await GetByCondition(d => d.NetworkId == brandIdentitySearchModel.NetworkId
                         && d.BrandSuffix == brandIdentitySearchModel.BrandSuffix)
                         .Include(d => d.Network).ToListAsync();
        }

        public async Task<List<Brand>> ListBrand()
        {
            return await GetAll()
                   .Include(d => d.Network)
                   .ToListAsync();
        }
        public async Task<bool> AddBrand(Brand brand)
        {
            await Create(brand);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteBrand(Brand brand)
        {
            Delete(brand);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateBrand(Brand brand)
        {
            Update(brand);
            await SaveChanges();
            return true;
        }
    }
}
