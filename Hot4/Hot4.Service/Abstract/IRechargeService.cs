using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IRechargeService
    {
        Task<RechargeDetailModel?> GetRechargeById(long rechargeId);
        Task<bool> UpdateRecharge(RechargeModel recharge);
        Task<bool> DeleteRecharge(long rechargeId);
        Task<bool> AddRecharge(RechargeModel recharge, long smsId);
        Task<bool> AddRechargeWithOutSmsDetails(RechargeModel rechargeModel);
        Task<List<RechargeDetailModel>> FindRechargeByMobileAndAccountId(RechargeFindModel rechargeFind);
        Task<List<RechargeDetailModel>> RechargePendingStsByMulBrands(List<byte> brandIds);
        Task<RechargeDetailModel?> RechargePendingStsByBrandId(byte brandId);
        Task<RechargeDetailModel?> GetRechargeWebDuplicate(RechWebDupSearchModel rechWebDup);
    }
}
