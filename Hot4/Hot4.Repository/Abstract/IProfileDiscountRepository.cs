using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IProfileDiscountRepository
    {
        Task<ProfileDiscount?> GetProfileDiscount(int discountId);
        Task<int> AddProfileDiscount(ProfileDiscount discount);
        Task UpdateProfileDiscount(ProfileDiscount discount);
        Task DeleteProfileDiscount(int discountID);


    }
}
