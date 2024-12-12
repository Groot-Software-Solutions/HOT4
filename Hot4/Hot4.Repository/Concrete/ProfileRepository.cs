using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProfileRepository : RepositoryBase<TblProfile>, IProfileRepository
    {
        public ProfileRepository(HotDbContext context) : base(context) { }
        public async Task<TblProfile?> GetProfile(int profileId)
        {
            return await GetById(profileId);
        }

        public async Task<List<TblProfile>> ListProfile()
        {
            return await GetAll().ToListAsync();
        }
    }
}
