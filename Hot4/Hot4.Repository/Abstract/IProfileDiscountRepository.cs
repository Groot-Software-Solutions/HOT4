using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProfileDiscountRepository
    {
        Task<ProfileDiscountModel?> GetPrfDiscountById(int prfDiscountId);
        Task<int> AddPrfDiscount(ProfileDiscount prfDiscount);
        Task UpdatePrfDiscount(ProfileDiscount prfDiscount);
        Task DeletePrfDiscount(ProfileDiscount prfDiscount);
        Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileId(int profileId);
        Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileBrandId(int profileId, int brandId);

    }
}
