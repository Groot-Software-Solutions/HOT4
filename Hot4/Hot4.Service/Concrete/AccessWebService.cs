using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
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
            var resut = await _accessWebRepository.GetAccessWebById(accessId);
            var mapper = _mapper.Map<AccessWebModel>(resut);
            return mapper;
            
        }
        public async Task<bool> AddAccessWeb(AccessWebModel accessWebModel)
        {
            var payloadMap = _mapper.Map<AccessWeb>(accessWebModel);
            var result =  _accessWebRepository.AddAccessWeb(payloadMap);
            return true;
        }

        public async Task<bool> UpdateAccessWeb(AccessWebModel accessWebModel)
        {
            var payloadMap = _mapper.Map<AccessWeb>(accessWebModel);
            var result =  _accessWebRepository.UpdateAccessWeb(payloadMap);
            return true;
        }

        public Task DeleteAccessWeb(AccessWebModel accessWebModel)
        {
            var payloadMap = _mapper.Map<AccessWeb>(accessWebModel);
            var result = _accessWebRepository.DeleteAccessWeb(payloadMap);
            return result;
        }
    }
}
