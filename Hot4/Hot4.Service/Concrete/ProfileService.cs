using AutoMapper;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Hot4.DataModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profile = Hot4.DataModel.Models.Profile;
using Hot4.Core.Enums;

namespace Hot4.Service.Concrete
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepository profileRepository, IMapper mapper)
        {
            _profileRepository = profileRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddProfile(ProfileModel profileModel)
        {
            if (profileModel != null)
            {
                var model = _mapper.Map<Profile>(profileModel);
                return await _profileRepository.AddProfile(model);
            }
            return false;
        }
        public async Task<bool> DeleteProfile(ProfileModel profileModel)
        {
            var record = await GetEntityById(profileModel.ProfileId);
            if (record != null)
            {
               return await _profileRepository.DeleteProfile(record);
            }
            return false;
        }
        public async Task<ProfileModel?> GetProfileById(int profileId)
        {
            var record = await GetEntityById(profileId);
            return _mapper.Map<ProfileModel>(record);
        }
        public async Task<List<ProfileModel>> ListProfile()
        {
            var records =await _profileRepository.ListProfile();
            return _mapper.Map<List<ProfileModel>>(records);
        }
        public async Task<bool> UpdateProfile(ProfileModel profileModel)
        {
            var record = await GetEntityById(profileModel.ProfileId);
            if (record != null)
            {
                _mapper.Map(profileModel , record);
                return await _profileRepository.AddProfile(record);
            }
            return false;
        }
        private async Task<Profile?> GetEntityById (int ProfileId)
        {
            return await _profileRepository.GetProfileById(ProfileId);
        }
    }
}
