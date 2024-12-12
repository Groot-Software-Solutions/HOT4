using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class AccessWebRepository : RepositoryBase<TblAccessWeb>, IAccessWebRepository
    {
        public AccessWebRepository(HotDbContext context) : base(context) { }
        public async Task AddAccessWeb(TblAccessWeb accessWeb)
        {
            await Create(accessWeb);
            await SaveChanges();
        }

        public async Task<TblAccessWeb?> GetAccessWeb(long accessId)
        {
            return await GetById(accessId);
        }

        public async Task UpdateAccessWeb(TblAccessWeb accessWeb)
        {
            await Update(accessWeb);
            await SaveChanges();
        }
    }
}
