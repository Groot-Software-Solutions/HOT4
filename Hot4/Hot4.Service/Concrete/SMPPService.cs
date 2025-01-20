using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class SMPPService : ISMPPService
    {
        private ISMPPRepository _smppRepository;
        private readonly IMapper Mapper;
        public SMPPService(ISMPPRepository smppRepository, IMapper mapper)
        {
            _smppRepository = smppRepository;
            Mapper = mapper;
        }
        public Task<bool> AddSMPP(SMPPModel smpp)
        {
            var model = Mapper.Map<Smpp>(smpp);
            return _smppRepository.AddSMPP(model);
        }

        public async Task<bool> DeleteSMPP(byte smppId)
        {
            var record = await GetEntityById(smppId);
            if (record != null)
            {
                return await _smppRepository.DeleteSMPP(record);
            }
            return false;
        }

        public async Task<SMPPModel?> GetSMPPById(byte smppId)
        {
            var record = await GetEntityById(smppId);
            return Mapper.Map<SMPPModel>(record);
        }

        public async Task<List<SMPPModel>> ListSMPP()
        {
            var records = await _smppRepository.ListSMPP();
            return Mapper.Map<List<SMPPModel>>(records);
        }

        public async Task<bool> UpdateSMPP(SMPPModel smpp)
        {
            var record = await GetEntityById(smpp.SmppId);
            if (record != null)
            {
                Mapper.Map(smpp, record);
                return await _smppRepository.UpdateSMPP(record);
            }
            return false;
        }

        private async Task<Smpp?> GetEntityById (byte SmppId)
        {
            return await _smppRepository.GetSMPPById(SmppId);
        }
    }
}
