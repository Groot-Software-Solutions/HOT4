using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Repository.Concrete;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class AccessWebService : IAccessWebService
    {
        private readonly IAccessWebRepository _accessWebRepository;
        private readonly IMapper Mapper;

        public AccessWebService(IAccessWebRepository accessWebRepository , IMapper mapper)
        {
            _accessWebRepository = accessWebRepository;
            Mapper = mapper;
        }
        public async Task<AccessWebModel?> GetAccessWebById(long accessId)
        {
            var record = await GetEntityById(accessId);
            return Mapper.Map<AccessWebModel>(record);            
        }
        public async Task<bool> AddAccessWeb(AccessWebModel accessWebModel)
        {
            if (accessWebModel != null)
            {
                var model = Mapper.Map<AccessWeb>(accessWebModel);
               return await _accessWebRepository.AddAccessWeb(model);    
            }
            return false;
            
        }
        public async Task<bool> UpdateAccessWeb(AccessWebModel accessWebModel)
        {
            var record = await GetEntityById(accessWebModel.AccessId);
            if (record != null)
            {
                Mapper.Map(accessWebModel, record);
               return await _accessWebRepository.UpdateAccessWeb(record);
            }
            return false;            
        }
        public async Task<bool> DeleteAccessWeb(long AccessId)
        {
            var record = await GetEntityById(AccessId);
            if (record != null)
            {
               return await _accessWebRepository.DeleteAccessWeb(record);
            }
            return false;            
        }
        private async Task<AccessWeb?> GetEntityById (long AccessId)
        {
            return await _accessWebRepository.GetAccessWebById(AccessId);
        }
    }
}
