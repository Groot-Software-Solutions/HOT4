using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class RechargePrepaidService : IRechargePrepaidService
    {
        private readonly IRechargePrepaidRepository _rechargePrepaidRepository;
        private readonly IMapper Mapper;
        public RechargePrepaidService(IRechargePrepaidRepository rechargePrepaidRepository, IMapper mapper)
        {
            _rechargePrepaidRepository = rechargePrepaidRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddRechargePrepaid(RechargePrepaidModel rechargeprepaid)
        {
            if (rechargeprepaid != null)
            {
                var model = Mapper.Map<RechargePrepaid>(rechargeprepaid);
                return await _rechargePrepaidRepository.AddRechargePrepaid(model);
            }
            return false;
            
        }

        public async Task<RechargePrepaidModel?> GetRechargePrepaidById(long rechargeId)
        {
            var record = await GetEntityById(rechargeId);
            return Mapper.Map<RechargePrepaidModel>(record);
        }

        public async Task<bool> UpdateRechargePrepaid(RechargePrepaidModel rechargePrepaid)
        {
            var record = await GetEntityById(rechargePrepaid.RechargeId);
            if (record != null)
            {
                record = Mapper.Map(rechargePrepaid, record);
                return await _rechargePrepaidRepository.UpdateRechargePrepaid(record);
            }
            return false;
        }
        private async Task<RechargePrepaid?> GetEntityById(long rechargeId)
        {
            return await _rechargePrepaidRepository.GetRechargePrepaidById(rechargeId);
        }
    }
}
