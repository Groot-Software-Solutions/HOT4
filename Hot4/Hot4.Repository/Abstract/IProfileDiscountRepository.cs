using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProfileDiscountRepository
    {
        Task<ProfileDiscount?> GetPrfDiscountById(int prfDiscountId);
        Task<bool> AddPrfDiscount(ProfileDiscount prfDiscount);
        Task<bool> UpdatePrfDiscount(ProfileDiscount prfDiscount);
        Task<bool> DeletePrfDiscount(ProfileDiscount prfDiscount);
        Task<List<ProfileDiscount>> GetPrfDiscountByProfileId(int profileId);
        Task<List<ProfileDiscount>> GetPrfDiscountByProfileAndBrandId(int profileId, int brandId);

    }
}
