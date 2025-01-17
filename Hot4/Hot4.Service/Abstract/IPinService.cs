using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IPinService
    {
        Task<bool> AddPin(PinToDo pin);
        Task<List<PinDetailModel>> GetPinDetailByBatchId(long pinBatchId);
        Task<List<PinLoadedModel>> GetPinLoadedByBatchId(long pinBatchId);
        Task<List<PinLoadedModel>> GetPinStock();
        Task<List<PinLoadedModel>> GetPinStockPromo();
        Task<List<PinDetailModel>> PinRecharge(PinRechargePayload pinRecharge);
        Task<long> PinRechargePromo(PinRechargePromoPayload pinRechargePromo);
        Task<PinRedeemedPromoModel> PinRedeemedPromo(long accountId);
        Task<List<PinDetailModel>> GetPinRechargeByRechId(long rechargeId);
    }
}
