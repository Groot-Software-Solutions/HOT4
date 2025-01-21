using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IProfileService
    {
        Task<ProfileModel?> GetProfileById(int profileId);
        Task<List<ProfileModel>> ListProfile();
        Task<bool> DeleteProfile(ProfileModel profileModel);
        Task<bool> AddProfile(ProfileModel profileModel);
        Task<bool> UpdateProfile(ProfileModel profileModel);
    }
}
