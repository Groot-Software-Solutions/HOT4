using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BundleRepository : RepositoryBase<Bundle>, IBundleRepository
    {
        public BundleRepository(HotDbContext context) : base(context) { }
        public async Task<BundleModel?> GetBundlesById(int bundleId)
        {
            return await (from bundle in _context.Bundle
                          where bundle.BundleId == bundleId
                          join brand in _context.Brand on bundle.BrandId equals brand.BrandId
                          select new BundleModel
                          {
                              BundleId = bundle.BundleId,
                              BrandId = bundle.BrandId,
                              Name = bundle.Name,
                              Description = bundle.Description,
                              Amount = bundle.Amount,
                              ProductCode = bundle.ProductCode,
                              ValidityPeriod = bundle.ValidityPeriod,
                              Enabled = bundle.Enabled,
                              Network = brand.BrandName
                          }).FirstOrDefaultAsync();
        }
        public async Task AddBundle(Bundle bundle)
        {
            await Create(bundle);
            await SaveChanges();
        }
        public async Task DeleteBundle(Bundle bundle)
        {
            await Delete(bundle);
            await SaveChanges();
        }
        public async Task UpdateBundle(Bundle bundle)
        {
            await Update(bundle);
            await SaveChanges();
        }
        public async Task<List<BundleModel>> ListBundles()
        {
            return await (from bundle in _context.Bundle
                          where bundle.Enabled
                          join brand in _context.Brand on bundle.BrandId equals brand.BrandId

                          select new BundleModel
                          {
                              BundleId = bundle.BundleId,
                              BrandId = bundle.BrandId,
                              Name = bundle.Name,
                              Description = bundle.Description,
                              Amount = bundle.Amount,
                              ProductCode = bundle.ProductCode,
                              ValidityPeriod = bundle.ValidityPeriod,
                              Enabled = bundle.Enabled,
                              Network = brand.BrandName
                          }).OrderBy(d =>
        d.BundleId == 59 ? 3 :
        d.BundleId == 60 ? 4 :
        d.BundleId == 45 ? 5 :
        d.BundleId == 18 ? 6 :
        d.BundleId == 24 ? 7 :
        d.BundleId == 20 ? 8 :
        d.BundleId == 25 ? 9 :
        d.BundleId == 19 ? 10 :
        d.BundleId == 26 ? 12 :
        d.BundleId == 21 ? 13 :
        d.BundleId == 23 ? 14 :
        d.BundleId == 22 ? 15 :
        d.BundleId == 27 ? 17 :
        d.BundleId == 28 ? 18 :
        d.BundleId == 29 ? 19 :
        d.BundleId == 30 ? 20 :
        d.BundleId == 31 ? 22 :
        d.BundleId == 32 ? 23 :
        d.BundleId == 33 ? 24 :
        d.BundleId == 34 ? 25 :
        d.BundleId == 61 ? 27 :
        d.BundleId == 4 ? 28 :
        d.BundleId == 68 ? 29 :
        d.BundleId == 2 ? 30 :
        d.BundleId == 1 ? 31 :
        d.BundleId == 3 ? 32 :
        d.BundleId == 46 ? 33 :
        d.BundleId == 14 ? 35 :
        d.BundleId == 16 ? 36 :
        d.BundleId == 13 ? 37 :
        d.BundleId == 69 ? 38 :
        d.BundleId == 15 ? 39 :
        d.BundleId == 17 ? 40 :
        d.BundleId == 7 ? 42 :
        d.BundleId == 9 ? 43 :
        d.BundleId == 71 ? 44 :
        d.BundleId == 11 ? 45 :
        d.BundleId == 12 ? 46 :
        d.BundleId == 8 ? 47 :
        d.BundleId == 10 ? 48 :
        d.BundleId == 53 ? 50 :
        d.BundleId == 54 ? 51 :
        d.BundleId == 55 ? 52 :
        d.BundleId == 56 ? 53 :
        d.BundleId == 57 ? 54 :
        d.BundleId == 58 ? 55 :
        d.BundleId == 47 ? 58 :
        d.BundleId == 48 ? 59 :
        d.BundleId == 49 ? 60 :
        d.BundleId == 66 ? 61 :
        d.BundleId == 50 ? 62 :
        d.BundleId == 51 ? 63 :
        d.BundleId == 52 ? 64 :
        d.BundleId == 67 ? 65 :
        d.BundleId == 35 ? 67 :
        d.BundleId == 36 ? 68 :
        d.BundleId == 37 ? 69 :
        d.BundleId == 72 ? 70 :
        d.BundleId == 73 ? 71 :
        d.BundleId == 86 ? 71 :
        d.BundleId == 87 ? 72 :
        d.BundleId == 88 ? 73 :
        d.BundleId == 79 ? 74 :
        d.BundleId == 83 ? 75 :
        d.BundleId == 84 ? 76 :
        d.BundleId == 85 ? 77 :
        d.BundleId == 74 ? 78 :
        d.BundleId == 76 ? 79 :
        d.BundleId == 78 ? 80 :
        200) // Default case
    .ThenBy(b => b.BrandId)
    .ThenBy(b => b.ValidityPeriod)
    .ThenBy(b => b.Amount)
    .ThenBy(b => b.Name)
    .ToListAsync();
        }
    }
}
