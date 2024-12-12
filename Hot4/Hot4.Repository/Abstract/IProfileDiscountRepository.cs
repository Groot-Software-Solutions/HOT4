using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IProfileDiscountRepository
    {
        Task<TblProfileDiscount?> GetProfileDiscount(int discountId);
        Task<int> AddProfileDiscount(TblProfileDiscount discount);
        Task UpdateProfileDiscount(TblProfileDiscount discount);
        Task DeleteProfileDiscount(int discountID);


    }
}
