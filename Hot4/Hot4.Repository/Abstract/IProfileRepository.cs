using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IProfileRepository
    {
        Task<Profile?> GetProfile(int profileId);

        Task<List<Profile>> ListProfile();
    }
}
