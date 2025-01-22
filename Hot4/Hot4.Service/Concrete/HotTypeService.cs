using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class HotTypeService : IHotTypeService
    {
        private readonly IHotTypeRepository _hotTypeRepository;
        private readonly IMapper Mapper;
        public HotTypeService(IHotTypeRepository hotTypeRepository, IMapper mapper)
        {
            _hotTypeRepository = hotTypeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddHotType(HotTypeRecord hotTypeModel)
        {
            if (hotTypeModel != null)
            {
                var model = Mapper.Map<HotTypes>(hotTypeModel);
                return await _hotTypeRepository.AddHotType(model);
            }
            return false;
        }
        public async Task<bool> DeleteHotType(byte hotTypeId)
        {
            var record = await GetEntityById(hotTypeId);
            if (record != null)
            {
                return await _hotTypeRepository.DeleteHotType(record);
            }
            return false;
        }
        public Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount)
        {
            return _hotTypeRepository.GetHotTypeIdentity(typeCode, splitCount);
        }
        public async Task<List<HotTypeModel>> ListHotType()
        {
            var records = await _hotTypeRepository.ListHotType();
            return Mapper.Map<List<HotTypeModel>>(records);
        }
        public async Task<bool> UpdateHotType(HotTypeRecord hotTypeModel)
        {
            var record = await GetEntityById(hotTypeModel.HotTypeId);
            if (record != null)
            {
                Mapper.Map(hotTypeModel, record);
                return await _hotTypeRepository.UpdateHotType(record);
            }
            return false;
        }
        public async Task<HotTypeModel> GetHotTypeById(byte hotTypeId)
        {
            var record = await GetEntityById(hotTypeId);
            return Mapper.Map<HotTypeModel>(record);
        }
        private async Task<HotTypes?> GetEntityById(byte hotTypeId)
        {
            return await _hotTypeRepository.GetHotTypeById(hotTypeId);
        }

    }
}
