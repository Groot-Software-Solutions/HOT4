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
    public class ConfigService : IConfigService
    {
        private readonly IConfigRepository _configRepository;
        private readonly IMapper _mapper;
        public ConfigService(IConfigRepository configRepository , IMapper mapper)
        {
            _configRepository = configRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddConfig(ConfigModel configModel)
        {
            if (configModel != null)
            {
                var model = _mapper.Map<Configs>(configModel);
             return  await _configRepository.AddConfig(model);
            }
            return false;
        } 
        public async Task<bool> DeleteConfig(ConfigModel configModel)
        {
            var record = await _configRepository.GetConfigById(configModel.ConfigId);
            if (record != null)
            {
             return  await _configRepository.DeleteConfig(record);
            }
            return false;
        }
        public async Task<ConfigModel> GetConfigById(byte ConfigId)
        {
            var record = await _configRepository.GetConfigById(ConfigId);
            if (record != null)
            {
                return _mapper.Map<ConfigModel>(record);
            }
            return null;
        }
        public async Task<List<ConfigModel>> ListConfig()
        {
            var records = await _configRepository.ListConfig();
            return  _mapper.Map<List<ConfigModel>>(records);
            
        }
        public async Task<bool> UpdateConfig(ConfigModel configModel)
        {
            var record = await _configRepository.GetConfigById(configModel.ConfigId);
            if (record != null) 
            {
                _mapper.Map(configModel, record);
              return await _configRepository.UpdateConfig(record);
            }
            return false;
        }
    }
}
