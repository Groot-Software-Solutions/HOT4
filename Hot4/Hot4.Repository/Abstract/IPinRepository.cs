using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPinRepository
    {
        Task<long> AddPin(Pins pin);
        Task<List<PinDetailModel>> GetPinDetailByBatchId(long pinBatchId);
        Task<List<PinLoadedModel>> GetPinLoadedByBatchId(long pinBatchId);
        Task<List<PinLoadedModel>> GetPinStock();
        Task<List<PinLoadedModel>> GetPinStockPromo();
        Task<List<PinDetailModel>> PinRecharge(PinRechargePayload pinRecharge);
        Task<long> PinRechargePromo(PinRechargePromoPayload pinRechargePromo);
        Task<PinRedeemedPromoModel> PinRedeemedPromo(long accountId);
    }
}
