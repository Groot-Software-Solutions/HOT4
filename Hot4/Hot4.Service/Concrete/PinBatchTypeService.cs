using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class PinBatchTypeService : IPinBatchTypeService
    {
        private IPinBatchTypeRepository _pinBatchTypeRepository;
        private IMapper Mapper;
        public PinBatchTypeService(IPinBatchTypeRepository pinBatchTypeRepository, IMapper mapper)
        {
            _pinBatchTypeRepository = pinBatchTypeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddPinBatchType(PinBatchTypeModel pinBatchTypes)
        {
            var model = Mapper.Map<PinBatchTypes>(pinBatchTypes);
            return await _pinBatchTypeRepository.AddPinBatchType(model);
        }

        public async Task<bool> DeletePinBatchType(PinBatchTypeModel pinBatchTypes)
        {
            var record = await GetEntityById(pinBatchTypes.PinBatchTypeId);
            if (record != null)
            {
                return await _pinBatchTypeRepository.DeletePinBatchType(record);
            }
            return false;
        }

        public async Task<List<PinBatchTypeModel>> ListPinBatchType()
        {
            var records = await _pinBatchTypeRepository.ListPinBatchType();
            return Mapper.Map<List<PinBatchTypeModel>>(records);
        }

        public async Task<bool> UpdatePinBatchType(PinBatchTypeModel pinBatchTypes)
        {
            var record = await GetEntityById(pinBatchTypes.PinBatchTypeId);
            if (record != null)
            {
                Mapper.Map(pinBatchTypes, record);
                return await _pinBatchTypeRepository.UpdatePinBatchType(record);
            }
            return false;
        }

        private async Task<PinBatchTypes?> GetEntityById(byte PinBatchTypeId)
        {
            return await _pinBatchTypeRepository.GetPinBatchTypeById(PinBatchTypeId);
        }
    }
}
