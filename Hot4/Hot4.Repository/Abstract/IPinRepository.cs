using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPinRepository
    {
        Task<bool> AddPin(Pins pin);
        Task<List<Pins>> GetPinDetailByBatchId(long pinBatchId);
        Task<List<PinLoadedModel>> GetPinLoadedByBatchId(long pinBatchId);
        Task<List<PinLoadedModel>> GetPinStock();
        Task<List<PinLoadedModel>> GetPinStockPromo();
        Task<List<Pins>> PinRecharge(PinRechargePayload pinRecharge);
        Task<long> PinRechargePromo(PinRechargePromoPayload pinRechargePromo);
        //   Task<PinRedeemedPromoModel> PinRedeemedPromo(long accountId);
        Task<int> PinRedeemedPromo(long accountId);
        Task<List<Pins>> GetPinRechargeByRechId(long rechargeId);
    }
}
