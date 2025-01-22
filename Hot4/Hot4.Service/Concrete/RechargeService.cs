using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class RechargeService : IRechargeService
    {
        private readonly IRechargeRepository _rechargeRepository;
        private readonly IMapper Mapper;
        public RechargeService(IRechargeRepository rechargeRepository, IMapper mapper)
        {
            _rechargeRepository = rechargeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddRecharge(RechargeModel recharge, long smsId)
        {
            if (recharge != null && smsId != null)
            {
                var model = Mapper.Map<Recharge>(recharge);
                return await _rechargeRepository.AddRecharge(model, smsId);
            }
            return false;
        }
        public async Task<bool> DeleteRecharge(long rechargeId)
        {
            var record = await GetEntityById(rechargeId);
            if (record != null)
            { return await _rechargeRepository.DeleteRecharge(record); }
            return false;
        }
        public async Task<List<RechargeDetailModel>> FindRechargeByMobileAndAccountId(RechargeFindModel rechargeFind)
        {
            var records = await _rechargeRepository.FindRechargeByMobileAndAccountId(rechargeFind);
            return Mapper.Map<List<RechargeDetailModel>>(records);
        }
        private async Task<Recharge?> GetEntityById(long rechargeId)
        {
            return await _rechargeRepository.GetRechargeById(rechargeId);
        }
        public async Task<RechargeDetailModel?> GetRechargeById(long rechargeId)
        {
            var record = await GetEntityById(rechargeId);
            return Mapper.Map<RechargeDetailModel?>(record);
        }
        public async Task<RechargeDetailModel?> GetRechargeWebDuplicate(RechWebDupSearchModel rechWebDup)
        {
            var record = await _rechargeRepository.GetRechargeWebDuplicate(rechWebDup);
            return Mapper.Map<RechargeDetailModel?>(record);
        }
        public async Task<RechargeDetailModel?> RechargePendingStsByBrandId(byte brandId)
        {
            var record = await _rechargeRepository.RechargePendingStsByBrandId(brandId);
            return Mapper.Map<RechargeDetailModel?>(record);
        }
        public async Task<List<RechargeDetailModel>> RechargePendingStsByMulBrands(List<byte> brandIds)
        {
            var records = await _rechargeRepository.RechargePendingStsByMulBrands(brandIds);
            return Mapper.Map<List<RechargeDetailModel>>(records);
        }
        public async Task<bool> UpdateRecharge(RechargeModel recharge)
        {
            var record = await GetEntityById(recharge.RechargeId);
            if (record != null)
            {
                Mapper.Map(recharge, record);
                return await _rechargeRepository.UpdateRecharge(record);
            }
            return false;
        }
    }
}
