using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProfileRepository
    {
        Task<Profile?> GetProfileById(int profileId);
        Task<List<Profile>> ListProfile();
        Task<bool> DeleteProfile(Profile profile);
        Task<bool> AddProfile(Profile profile);
        Task<bool> UpdateProfile(Profile profile);
    }
}
