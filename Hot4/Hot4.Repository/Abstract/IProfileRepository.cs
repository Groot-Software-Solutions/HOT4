using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IProfileRepository
    {
        Task<TblProfile?> GetProfile(int profileId);

        Task<List<TblProfile>> ListProfile();
    }
}
