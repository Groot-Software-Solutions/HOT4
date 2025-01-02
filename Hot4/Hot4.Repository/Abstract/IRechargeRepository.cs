using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IRechargeRepository
    {
        Task<RechargeDetailModel?> GetRechargeById(long rechargeId);
        Task UpdateRecharge(Recharge recharge);
        Task AddRecharge(Recharge recharge, long smsId);
        Task<List<RechargeModel>> GetRechargeAggregator(RechargeAggSearchModel rechargeAggSearch);
        Task<List<RechargeDetailModel>> FindRechargeByMobileAndAccountId(RechargeFindModel rechargeFind);
        Task<List<RechargeModel>> UpdatePendingStsByMulBrands(List<byte> brandIds);
        Task<RechargeDetailModel?> UpdatePendingStsByBrandId(byte brandId);
        Task<RechargeDetailModel?> UpdatePendingStsOtherBrand();
        Task<RechargeModel?> GetRechargeWebDuplicate(RechWebDupSearchModel rechWebDup);
    }
}
