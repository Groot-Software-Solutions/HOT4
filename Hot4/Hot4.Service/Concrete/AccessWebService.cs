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
        private readonly IMapper _mapper;

        public AccessWebService(IAccessWebRepository accessWebRepository , IMapper mapper)
        {
            _accessWebRepository = accessWebRepository;
            _mapper = mapper;
        }
        public async Task<AccessWebModel?> GetAccessWebById(long accessId)
        {
            var record = await _accessWebRepository.GetAccessWebById(accessId);
            var model = _mapper.Map<AccessWebModel>(record);
            return model;
            
        }
        public async Task<bool> AddAccessWeb(AccessWebModel accessWebModel)
        {
            if (accessWebModel != null)
            {
                var model = _mapper.Map<AccessWeb>(accessWebModel);
               return await _accessWebRepository.AddAccessWeb(model);    
            }
            return false;
            
        }
        public async Task<bool> UpdateAccessWeb(AccessWebModel accessWebModel)
        {
            var record = await _accessWebRepository.GetAccessWebById(accessWebModel.AccessId);
            if (record != null)
            {
                _mapper.Map(accessWebModel, record);
               return await _accessWebRepository.UpdateAccessWeb(record);
            }
            return false;
            
        }
        public async Task<bool> DeleteAccessWeb(AccessWebModel accessWebModel)
        {
            var record = await _accessWebRepository.GetAccessWebById(accessWebModel.AccessId);
            if (record != null)
            {
               return await _accessWebRepository.DeleteAccessWeb(record);
            }
            return false;
            
        }
    }
}
