using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Microsoft.Identity.Client;
using System.Net.Http.Headers;

namespace Hot4.Service.Concrete
{
    public class AccessService : IAccessService
    {
        private readonly IAccessRepository _accessRepository;
        private readonly IMapper _mapper;

        public AccessService(IAccessRepository accessRepository , IMapper mapper)
        {
            _accessRepository = accessRepository;
            _mapper = mapper;
        }


        public async Task<AccessModel?> GetAccessById(long accessId)
        {
            var accessById = await _accessRepository.GetAccessById(accessId);
            return _mapper.Map<AccessModel>(accessById);
        }


        public async Task<List<AccessModel>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId)
        {
            var getAccessByAccountIdAndChannelId = await _accessRepository.GetAccessByAccountIdAndChannelId(accountId, channelId);
            return _mapper.Map<List<AccessModel>>(getAccessByAccountIdAndChannelId);
            
        }
            


        public async Task<AccountAccessModel?> GetAccessByCode(string accessCode)
        {
            var getAccessByCode = await _accessRepository.GetAccessByCode(accessCode);
            if (getAccessByCode != null)
            {
                var mapperAccountAccessModel = _mapper.Map<AccountAccessModel>(getAccessByCode);
                mapperAccountAccessModel.AccessPassword = "********";
                mapperAccountAccessModel.PasswordHash = "********";
                return mapperAccountAccessModel;             
            }

            return null;
        }

        public  async Task<bool> AddAccess(AccessModel accessModel)
        {
            var accessMapper = _mapper.Map<Access>(accessModel);
            await  _accessRepository.AddAccess(accessMapper);
            return true;
        }   
       

        public async Task<bool> AddAccessDeprecated(AccessModel accessModel)
        {
            var accessMapper = _mapper.Map<Access>(accessModel);
            await _accessRepository.AddAccessDeprecated(accessMapper);
            return true;
        }
            
       

        public async Task<bool> UpdateAccess(AccessModel accessModel)
        {

            var accessMapper = _mapper.Map<Access>(accessModel);
            await _accessRepository.UpdateAccess(accessMapper);
            return true;


        }

        public async Task<List<AccountAccessModel>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted)
        {
            var accessByAccountId = await _accessRepository.GetAccessByAccountId(accountId, isGetAll, isDeleted);

            if (accessByAccountId != null) { 
                var mappedList =  _mapper.Map<List<AccountAccessModel>>(accessByAccountId);
                foreach (var accessModel in mappedList)
                {
                    accessModel.AccessPassword = "********";
                    accessModel.PasswordHash = "********";
                } 
                return mappedList;
            }
            return null;

        }

        public Task<long> GetAdminId(long accountId) => _accessRepository.GetAdminId(accountId);

        public Task PasswordChange(long accessId, string newPassword) => _accessRepository.PasswordChange(accessId, newPassword);

        public Task PasswordChangeDeprecated(long accessId, string passwordHash, string passwordSalt) => _accessRepository.PasswordChangeDeprecated(accessId, passwordHash, passwordSalt);

        public async Task<AccountAccessModel?> GetLoginDetails(string accessCode, string accessPassword)
        {
            var getLoginDetails = await _accessRepository.GetLoginDetails(accessCode, accessPassword);

            if (getLoginDetails != null)
            {
                var mapperAccountAccessModel = _mapper.Map<AccountAccessModel>(getLoginDetails);

                mapperAccountAccessModel.AccessPassword = "********";
                mapperAccountAccessModel.PasswordHash = "********";
                return mapperAccountAccessModel;

            }

            return null;
        }
            
            

        public async Task<AccountAccessModel?> GetLoginDetailsByAccessCode(string accessCode)
        {
            var getLoginDetailsByAccessCode  = await _accessRepository.GetLoginDetailsByAccessCode(accessCode);

            if (getLoginDetailsByAccessCode != null)
            {
                var mapperAccountAccessModel = _mapper.Map<AccountAccessModel>(getLoginDetailsByAccessCode);

                mapperAccountAccessModel.AccessPassword = "********";
                mapperAccountAccessModel.PasswordHash = "********";

                return mapperAccountAccessModel;
            }

            return null;

        }
            

        public Task DeleteAccess(long accessId) => _accessRepository.DeleteAccess(accessId);
      

        public Task UnDeleteAccess(long accessId) => _accessRepository.UnDeleteAccess(accessId);
        
    }
}
