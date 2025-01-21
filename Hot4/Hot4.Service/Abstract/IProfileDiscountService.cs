using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IProfileDiscountService
    {
        Task<ProfileDiscountModel?> GetPrfDiscountById(int ProfileDiscountId);
        Task<bool> AddPrfDiscount(ProfileDiscountModel profileDiscountModel);
        Task<bool> UpdatePrfDiscount(ProfileDiscountModel profileDiscountModel);
        Task<bool> DeletePrfDiscount(int ProfileDiscountId);
        Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileId(int profileId);
        Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileAndBrandId(int profileId, int brandId);

    }
}
