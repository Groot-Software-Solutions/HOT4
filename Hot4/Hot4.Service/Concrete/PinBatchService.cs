using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class PinBatchService : IPinBatchService
    {
        private readonly IPinBatchRepository _pinBatchRepository;
        private readonly IMapper Mapper;
        public PinBatchService(IPinBatchRepository pinBatchRepository, IMapper mapper)
        {
            _pinBatchRepository = pinBatchRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddPinBatch(PinBatchRecord pinBatches)
        {
            if (pinBatches != null)
            {
                var model = Mapper.Map<PinBatches>(pinBatches);
                return await _pinBatchRepository.AddPinBatch(model);
            }
            return false;
        }

        public async Task<bool> DeletePinBatch(PinBatchRecord pinBatches)
        {
            var record = await GetEntityById(PinBatchId);
            if (record != null)
            {
                return await _pinBatchRepository.DeletePinBatch(record);
            }
            return false;
        }
        public async Task<PinBatchModel?> GetPinBatchById(long pinBatchId)
        {
            var record = await GetEntityById(pinBatchId);
            return Mapper.Map<PinBatchModel>(record);
        }
        public async Task<List<PinBatchModel>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId)
        {
            var records = await _pinBatchRepository.GetPinBatchByPinBatchTypeId(pinBatchTypeId);
            return Mapper.Map<List<PinBatchModel>>(records);
        }

        public async Task<bool> UpdatePinBatch(PinBatchRecord pinBatches)
        {
            var record = await GetEntityById(pinBatches.PinBatchId);
            if (record != null)
            {
                Mapper.Map(pinBatches, record);
                return await _pinBatchRepository.UpdatePinBatch(record);
            }
            return false;
        }
        private async Task<PinBatches?> GetEntityById (long PinBatchId)
        {
            return await _pinBatchRepository.GetPinBatchById(PinBatchId);
        }
    }
}
