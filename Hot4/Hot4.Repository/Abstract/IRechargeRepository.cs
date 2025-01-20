using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IRechargeRepository
    {
        Task<Recharge?> GetRechargeById(long rechargeId);
        Task<bool> UpdateRecharge(Recharge recharge);
        Task<bool> DeleteRecharge(Recharge recharge);
        Task<bool> AddRecharge(Recharge recharge, long smsId);
        Task<List<Recharge>> FindRechargeByMobileAndAccountId(RechargeFindModel rechargeFind);
        Task<List<Recharge>> RechargePendingStsByMulBrands(List<byte> brandIds);
        Task<Recharge?> RechargePendingStsByBrandId(byte brandId);
        Task<Recharge?> GetRechargeWebDuplicate(RechWebDupSearchModel rechWebDup);
    }
}
