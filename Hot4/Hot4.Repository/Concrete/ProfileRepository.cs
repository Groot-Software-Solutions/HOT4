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

        public async Task AddProfile(Profile profile)
        {
            await Create(profile);
            await SaveChanges();
        }

        public async Task DeleteProfile(Profile profile)
        {
            await Delete(profile);
            await SaveChanges();
        }

        public async Task<ProfileModel?> GetProfile(int profileId)
        {
            var result = await GetById(profileId);
            if (result != null)
            {
                return new ProfileModel
                {
                    ProfileId = result.ProfileId,
                    ProfileName = result.ProfileName,
                };
            }
            return null;
        }

        public async Task<List<ProfileModel>> ListProfile()
        {
            return await GetAll()
                .Select(d => new ProfileModel
                {
                    ProfileId = d.ProfileId,
                    ProfileName = d.ProfileName,
                })
                .ToListAsync();
        }

        public async Task UpdateProfile(Profile profile)
        {
            await Update(profile);
            await SaveChanges();
        }
    }
}
