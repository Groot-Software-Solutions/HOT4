using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProfileDiscountRepository : RepositoryBase<ProfileDiscount>, IProfileDiscountRepository
    {
        public ProfileDiscountRepository(HotDbContext context) : base(context) { }

        public async Task<ProfileDiscountModel?> GetPrfDiscountById(int prfDiscountId)
        {
            var result = await _context.ProfileDiscount
                               .Include(d => d.Brand)
                               .ThenInclude(d => d.Network)
                               .FirstOrDefaultAsync(d => d.ProfileDiscountId == prfDiscountId);
            if (result != null)
            {
                return new ProfileDiscountModel
                {
                    ProfileDiscountId = result.ProfileDiscountId,
                    Discount = result.Discount,
                    ProfileId = result.ProfileId,
                    BrandId = result.BrandId,
                    BrandName = result.Brand.BrandName,
                    BrandSuffix = result.Brand.BrandSuffix,
                    NetworkId = result.Brand.NetworkId,
                    Network = result.Brand.Network.Network,
                    NetworkPrefix = result.Brand.Network.Prefix,

                };
            }
            return null;
        }
        public async Task<int> AddPrfDiscount(ProfileDiscount profileDiscount)
        {
            await Create(profileDiscount);
            await SaveChanges();
            return profileDiscount.ProfileDiscountId;
        }
        public async Task UpdatePrfDiscount(ProfileDiscount profileDiscount)
        {
            Update(profileDiscount);
            await SaveChanges();
        }

        public async Task DeletePrfDiscount(ProfileDiscount profileDiscount)
        {
            Delete(profileDiscount);
            await SaveChanges();
        }
        public async Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileId(int profileId)
        {
            return await GetByCondition(d => d.ProfileId == profileId)
                         .Select(d => new ProfileDiscountModel
                         {
                             ProfileDiscountId = d.ProfileDiscountId,
                             Discount = d.Discount,
                             ProfileId = d.ProfileId,
                             BrandId = d.BrandId,
                             BrandName = d.Brand.BrandName,
                             BrandSuffix = d.Brand.BrandSuffix,
                             NetworkId = d.Brand.NetworkId,
                             Network = d.Brand.Network.Network,
                             NetworkPrefix = d.Brand.Network.Prefix,
                         }).ToListAsync();
        }
        public async Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileAndBrandId(int profileId, int brandId)
        {
            return await GetByCondition(d => d.ProfileId == profileId && d.BrandId == brandId)
                         .Include(d => d.Brand).ThenInclude(d => d.Network)
                         .Select(d => new ProfileDiscountModel
                         {
                             ProfileDiscountId = d.ProfileDiscountId,
                             Discount = d.Discount,
                             ProfileId = d.ProfileId,
                             BrandId = d.BrandId,
                             BrandName = d.Brand.BrandName,
                             BrandSuffix = d.Brand.BrandSuffix,
                             NetworkId = d.Brand.NetworkId,
                             Network = d.Brand.Network.Network,
                             NetworkPrefix = d.Brand.Network.Prefix,

                         }).ToListAsync();

        }

    }
}
