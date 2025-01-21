using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Concrete
{
    public class HotTypeService : IHotTypeService
    {
        private readonly IHotTypeRepository _hotTypeRepository;
        private readonly IMapper Mapper;
        public HotTypeService(IHotTypeRepository hotTypeRepository ,IMapper mapper)
        {
            _hotTypeRepository = hotTypeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddHotType(HotTypeModel hotTypeModel)
        {
            if (hotTypeModel != null)
            {
                var model =  Mapper.Map<HotTypes>(hotTypeModel);
                return await _hotTypeRepository.AddHotType(model);
            }
            return false;
        }
        public async Task<bool> DeleteHotType(byte HotTypeId)
        {
            var record =  await GetEntityById(HotTypeId);
            if (record != null) 
            {
            return  await _hotTypeRepository.DeleteHotType(record);
            }
            return false;
        }
        // need to check GetHotTypeIdentity
        public Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount)
        {
            return _hotTypeRepository.GetHotTypeIdentity(typeCode, splitCount);
        }
        // need to chcek ListHotType()
        public Task<List<HotTypeModel>> ListHotType()
        {
            return _hotTypeRepository.ListHotType();
        }
        public async Task<bool> UpdateHotType(HotTypeModel hotTypeModel)
        {
            var record = await GetEntityById(hotTypeModel.HotTypeId);
            if (record != null) 
            {
                Mapper.Map(hotTypeModel,record);
                return await _hotTypeRepository.UpdateHotType(record);
            }
            return false;
        }
        public async Task<HotTypeModel> GetHotTypeById(byte HotTypeId)
        {
            var record = await GetEntityById(HotTypeId);
            return Mapper.Map<HotTypeModel>(record);
        }
        private async Task<HotTypes> GetEntityById(byte HotTypeId)
        {
            return await _hotTypeRepository.GetHotTypeById(HotTypeId);
        }

    }
}
