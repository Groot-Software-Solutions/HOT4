using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProfileRepository : RepositoryBase<Profile>, IProfileRepository
    {
        public ProfileRepository(HotDbContext context) : base(context) { }
        public async Task<Profile?> GetProfile(int profileId)
        {
            return await GetById(profileId);
        }

        public async Task<List<Profile>> ListProfile()
        {
            return await GetAll().ToListAsync();
        }
    }
}
