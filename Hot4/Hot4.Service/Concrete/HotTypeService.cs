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
        private readonly IMapper _mapper;

        public HotTypeService(IHotTypeRepository hotTypeRepository ,IMapper mapper)
        {
            _hotTypeRepository = hotTypeRepository;
            _mapper = mapper;

        }
        public async Task<bool> AddHotType(HotTypeModel hotTypeModel)
        {
            if (hotTypeModel != null)
            {
                var model =  _mapper.Map<HotTypes>(hotTypeModel);
                return await _hotTypeRepository.AddHotType(model);
            }
            return false;
        }

        public async Task<bool> DeleteHotType(HotTypeModel hotTypeModel)
        {
            var record =  await GetEntityById(hotTypeModel.HotTypeId);
            if (record != null) 
            {
                var model = _mapper.Map<HotTypes>(record);
            return  await _hotTypeRepository.DeleteHotType(model);
            }
            return false;
        }

        public Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount)
        {
            return _hotTypeRepository.GetHotTypeIdentity(typeCode, splitCount);
        }

        public Task<List<HotTypeModel>> ListHotType()
        {
            return _hotTypeRepository.ListHotType();
        }

        public async Task<bool> UpdateHotType(HotTypeModel hotTypeModel)
        {
            var record = await GetEntityById(hotTypeModel.HotTypeId);
            if (record != null) 
            {
                var model = _mapper.Map<HotTypes>(record);
                return await _hotTypeRepository.UpdateHotType(model);
            }
            return false;
        }

        public async Task<HotTypeModel> GetHotTypeById(byte HotTypeId)
        {
            var record = await _hotTypeRepository.GetHotTypeById(HotTypeId);
            return _mapper.Map<HotTypeModel>(record);
        }

        private async Task<HotTypeModel> GetEntityById(byte HotTypeId)
        {
            var record = await GetHotTypeById(HotTypeId);
            if (record != null)
            {
                return record;
            }
            return null;
        }
    }
}
