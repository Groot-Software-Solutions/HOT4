using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IRechargeRepository
    {
        Task<RechargeDetailModel?> GetRecharge(long rechargeId);
        Task UpdateRecharge(Recharge recharge);
        Task InsertRecharge(Recharge recharge, long smsId);
        Task<List<RechargeModel>> GetRechargeAggregator(RechargeAggSearchModel rechargeAggSearch);
        Task<List<RechargeDetailModel>> RechargeFindByMobileAccountId(RechargeFindModel rechargeFind);
        Task<List<RechargeModel>> RechargePending(List<byte> brandIds);
        Task<RechargeDetailModel?> RechargePendingByBrandId(byte brandId);
        Task<RechargeDetailModel?> RechargePendingOtherBrand();
        Task<RechargeModel?> GetRechargeWebDuplicate(RechWebDupSearchModel rechWebDup);
    }
}
