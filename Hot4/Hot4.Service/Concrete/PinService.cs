using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class PinService : IPinService
    {
        private readonly IPinRepository _pinRepository;
        private readonly IMapper Mapper;
        public PinService(IPinRepository pinRepository, IMapper mapper)
        {
            _pinRepository = pinRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddPin(PinRecord pin)
        {
            if (pin != null)
            {
                var model = Mapper.Map<Pins>(pin);
                return await _pinRepository.AddPin(model);
            }
            return false;
        }
        public async Task<List<PinDetailModel>> GetPinDetailByBatchId(long pinBatchId)
        {
            var records = await _pinRepository.GetPinDetailByBatchId(pinBatchId);
            return Mapper.Map<List<PinDetailModel>>(records);
        }
        public async Task<List<PinLoadedModel>> GetPinLoadedByBatchId(long pinBatchId)
        {
            return await _pinRepository.GetPinLoadedByBatchId(pinBatchId);
        }
        public async Task<List<PinDetailModel>> GetPinRechargeByRechId(long rechargeId)
        {
            var records = await _pinRepository.GetPinRechargeByRechId(rechargeId);
            return Mapper.Map<List<PinDetailModel>>(records);
        }
        public async Task<List<PinLoadedModel>> GetPinStock()
        {
            return await _pinRepository.GetPinStock();
        }
        public async Task<List<PinLoadedModel>> GetPinStockPromo()
        {
            return await _pinRepository.GetPinStockPromo();
        }
        public async Task<List<PinDetailModel>> PinRecharge(PinRechargePayload pinRecharge)
        {
            var records = await _pinRepository.PinRecharge(pinRecharge);
            return Mapper.Map<List<PinDetailModel>>(records);
        }
        public async Task<long> PinRechargePromo(PinRechargePromoPayload pinRechargePromo)
        {
            return await _pinRepository.PinRechargePromo(pinRechargePromo);
        }
        public async Task<PinRedeemedPromoModel> PinRedeemedPromo(long accountId)
        {
            var transactions = await _pinRepository.PinRedeemedPromo(accountId);
            if (transactions > 0)
            {
                return new PinRedeemedPromoModel
                {
                    HasPurchased = true
                };
            }
            else
            {
                return new PinRedeemedPromoModel
                {
                    HasPurchased = false
                };
            }
        }

    }
}
