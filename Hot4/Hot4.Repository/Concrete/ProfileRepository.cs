using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProfileRepository : RepositoryBase<Profile>, IProfileRepository
    {
        public ProfileRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddProfile(Profile profile)
        {
            await Create(profile);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteProfile(Profile profile)
        {
            Delete(profile);
            await SaveChanges();
            return true;
        }
        public async Task<Profile?> GetProfileById(int profileId)
        {
            return await GetById(profileId);
           
        }
        public async Task<List<Profile>> ListProfile()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<bool> UpdateProfile(Profile profile)
        {
            Update(profile);
            await SaveChanges();
            return true;    
        }
    }
}
