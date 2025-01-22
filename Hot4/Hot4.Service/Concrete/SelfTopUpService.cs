using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class SelfTopUpService : ISelfTopUpService
    {
        private readonly ISelfTopUpRepository _selfTopUpRepository;
        private readonly IMapper Mapper;
        public SelfTopUpService(ISelfTopUpRepository selfTopUpRepository, IMapper mapper)
        {
            _selfTopUpRepository = selfTopUpRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddSelfTopUp(SelfTopUpRecord selfTopUp)
        {
            if (selfTopUp != null)
            {
                var model = Mapper.Map<SelfTopUp>(selfTopUp);
                return await _selfTopUpRepository.AddSelfTopUp(model);
            }
            return false;            
        }
        public async Task<bool> DeleteSelfTopUp(long selfTopUpId)
        {
            var record = await GetEntityById(selfTopUpId);
            if (record != null)
            {
                return await _selfTopUpRepository.DeleteSelfTopUp(record);
            }
            return false;
        }
        public async Task<List<SelfTopUpModel>> GetSelfTopUpByBankTrxId(long bankTrxId)
        {
            var record = await _selfTopUpRepository.GetSelfTopUpByBankTrxId(bankTrxId);
            return Mapper.Map<List<SelfTopUpModel>>(record);
        }
        public async Task<SelfTopUpModel?> GetSelfTopUpById(long selfTopUpId)
        {
            var records = await GetEntityById(selfTopUpId);
            return Mapper.Map<SelfTopUpModel?>(records);
        }
        public async Task<List<SelfTopUpModel>> GetSelfTopUpByStateId(byte selfTopUpStateId)
        {
            var records = await _selfTopUpRepository.GetSelfTopUpByStateId(selfTopUpStateId);
            return Mapper.Map<List<SelfTopUpModel>>(records);
        }

        public async Task<bool> UpdateSelfTopUp(SelfTopUpRecord selfTopUp)
        {
            var record = await GetEntityById(selfTopUp.SelfTopUpId);
            if (record != null)
            {
                Mapper.Map(selfTopUp, record);
                return await _selfTopUpRepository.UpdateSelfTopUp(record);
            }
            return false;
        }
        private async Task<SelfTopUp?> GetEntityById(long selfTopUpId)
        {
            return await _selfTopUpRepository.GetSelfTopUpById(selfTopUpId);
        }
    }
}
