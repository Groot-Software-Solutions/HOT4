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
        private readonly IMapper Mapper;
        public AccessService(IAccessRepository accessRepository , IMapper mapper)
        {
            _accessRepository = accessRepository;
            Mapper = mapper;
        }
        public async Task<AccessModel?> GetAccessById(long accessId)
        {
            var record = await GetEntityById(accessId);
            return Mapper.Map<AccessModel>(record);
        }
        public async Task<List<AccessModel>> GetAccessByAccountIdAndChannelId(long accountId, byte channelId)
        {
            var records = await _accessRepository.GetAccessByAccountIdAndChannelId(accountId, channelId);
            return Mapper.Map<List<AccessModel>>(records);            
        }
        public async Task<AccountAccessModel?> GetAccessByCode(string accessCode)
        {
            var record = await _accessRepository.GetAccessByCode(accessCode);
            if (record != null)
            {
                var model = Mapper.Map<AccountAccessModel>(record);
                model.AccessPassword = "********";
                model.PasswordHash = "********";
                return model;             
            }

            return null;
        }
        public  async Task<bool> AddAccess(AccessModel accessModel)
        {
            if (accessModel != null )
            {
                var model = Mapper.Map<Access>(accessModel);
               return await _accessRepository.AddAccess(model);
               
            }
            return false;            
        }          
        public async Task<bool> AddAccessDeprecated(AccessModel accessModel)
        {
            if (accessModel != null )
            {
                var model = Mapper.Map<Access>(accessModel);
               return await _accessRepository.AddAccessDeprecated(model);
                
            }
            return false;            
        }           
        public async Task<bool> UpdateAccess(AccessModel accessModel)
        {
            var record =  await GetEntityById(accessModel.AccessId);           
            if (record != null)
            {               
                Mapper.Map(accessModel, record);
               return await _accessRepository.UpdateAccess(record);
               
            }
            return false; 
        }
        public async Task<List<AccountAccessModel>> GetAccessByAccountId(long accountId, bool isGetAll, bool isDeleted)
        {
            var records = await _accessRepository.GetAccessByAccountId(accountId, isGetAll, isDeleted);
            if (records != null) { 
                var model =  Mapper.Map<List<AccountAccessModel>>(records);
                foreach (var accessModel in model)
                {
                    accessModel.AccessPassword = "********";
                    accessModel.PasswordHash = "********";
                } 
                return model;
            }   
            return null;

        }
        public async Task<long> GetAdminId(long accountId) 
        { 
          return await _accessRepository.GetAdminId(accountId);     
        }
        public async Task<bool> PasswordChange(long accessId, string newPassword) 
        {
            var record = await GetEntityById(accessId);
            if (record != null)
            {
               return await _accessRepository.PasswordChange(record, accessId , newPassword);
            }
            return false;
        }
        public async Task<bool> PasswordChangeDeprecated(long accessId, string passwordHash, string passwordSalt)
        {
            var record = await GetEntityById(accessId);
            if (record != null)
            {
            return await _accessRepository.PasswordChangeDeprecated(record, passwordHash, passwordSalt);               
            }
            return false;
        }
        public async Task<AccountAccessModel?> GetLoginDetails(string accessCode, string accessPassword)
        {
            var record = await _accessRepository.GetLoginDetails(accessCode, accessPassword);
            if (record != null)
            {
                var model = Mapper.Map<AccountAccessModel>(record);                
                model.AccessPassword = "********";
                model.PasswordHash = "********";
                return model;
            }
            return null;
        }
        public async Task<AccountAccessModel?> GetLoginDetailsByAccessCode(string accessCode)
        {
            var record  = await _accessRepository.GetLoginDetailsByAccessCode(accessCode);
            if (record != null)
            {
                var model = Mapper.Map<AccountAccessModel>(record);
                model.AccessPassword = "********";
                model.PasswordHash = "********";
                return model;
            }
            return null;
        }
        public async Task<bool> DeleteAccess(long accessId) 
        {
            var record = await GetEntityById(accessId);
            if (record != null)
            {
              return await _accessRepository.DeleteAccess(record);  
            }
            return false;            
        }
        public async Task<bool> UnDeleteAccess(long accessId) 
        {
            var record = await GetEntityById(accessId);
            if (record != null)
            {
              return await _accessRepository.UnDeleteAccess(record);
            }
            return false;            
        }
        private async Task<Access?> GetEntityById(long AccessId)
        {
            return await _accessRepository.GetAccessById(AccessId);
        }

    }
}
