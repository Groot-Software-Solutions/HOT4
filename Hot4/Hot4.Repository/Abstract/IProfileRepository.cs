using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProfileRepository
    {
        Task<ProfileModel?> GetProfile(int profileId);
        Task<List<ProfileModel>> ListProfile();
        Task DeleteProfile(Profile profile);
        Task AddProfile(Profile profile);
        Task UpdateProfile(Profile profile);
    }
}
