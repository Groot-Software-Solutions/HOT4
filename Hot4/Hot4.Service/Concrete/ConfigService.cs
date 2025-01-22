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
        private readonly IMapper Mapper;
        public ConfigService(IConfigRepository configRepository , IMapper mapper)
        {
            _configRepository = configRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddConfig(ConfigModel configModel)
        {
            if (configModel != null)
            {
                var model = Mapper.Map<Configs>(configModel);
             return  await _configRepository.AddConfig(model);
            }
            return false;
        } 
        public async Task<bool> DeleteConfig(byte configId)
        {
            var record = await GetEntityById(configId);
            if (record != null)
            {
             return  await _configRepository.DeleteConfig(record);
            }
            return false;
        }
        public async Task<ConfigModel> GetConfigById(byte configId)
        {
            var record = await GetEntityById(configId); 
            return Mapper.Map<ConfigModel>(record);
        }
        public async Task<List<ConfigModel>> ListConfig()
        {
            var records = await _configRepository.ListConfig();
            return  Mapper.Map<List<ConfigModel>>(records);           
        }
        public async Task<bool> UpdateConfig(ConfigModel configModel)
        {
            var record = await GetEntityById(configModel.ConfigId);
            if (record != null) 
            {
                Mapper.Map(configModel, record);
              return await _configRepository.UpdateConfig(record);
            }
            return false;
        }
        private async Task<Configs> GetEntityById (byte configId)
        {
            return await _configRepository.GetConfigById(configId);
        }
    }
}
