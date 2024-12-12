using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class ProfileDiscountRepository : RepositoryBase<TblProfileDiscount>, IProfileDiscountRepository
    {
        public ProfileDiscountRepository(HotDbContext context) : base(context) { }

        public async Task<TblProfileDiscount?> GetProfileDiscount(int discountId)
        {
            return await GetById(discountId);
        }
        public async Task<int> AddProfileDiscount(TblProfileDiscount discount)
        {
            await Create(discount);
            await SaveChanges();
            return discount.ProfileDiscountId;
        }
        public async Task UpdateProfileDiscount(TblProfileDiscount discount)
        {

            await Update(discount);
            await SaveChanges();

        }

        public async Task DeleteProfileDiscount(int discountId)
        {
            var existing = await GetById(discountId);
            if (existing != null)
            {
                await Delete(existing);
                await SaveChanges();
            }
        }




    }
}
