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
        public async Task<ProfileDiscount?> GetPrfDiscountById(int prfDiscountId)
        {
            return await _context.ProfileDiscount
                               .Include(d => d.Brand)
                               .ThenInclude(d => d.Network)
                               .FirstOrDefaultAsync(d => d.ProfileDiscountId == prfDiscountId);          
        }
        public async Task<bool> AddPrfDiscount(ProfileDiscount profileDiscount)
        {
            await Create(profileDiscount);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdatePrfDiscount(ProfileDiscount profileDiscount)
        {
            Update(profileDiscount);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeletePrfDiscount(ProfileDiscount profileDiscount)
        {
            Delete(profileDiscount);
            await SaveChanges();
            return true;
        }
        public async Task<List<ProfileDiscount>> GetPrfDiscountByProfileId(int profileId)
        {
            return await GetByCondition(d => d.ProfileId == profileId).ToListAsync();
        }
        public async Task<List<ProfileDiscount>> GetPrfDiscountByProfileAndBrandId(int profileId, int brandId)
        {
            return await GetByCondition(d => d.ProfileId == profileId && d.BrandId == brandId)
                         .Include(d => d.Brand).ThenInclude(d => d.Network).ToListAsync();

        }

    }
}
