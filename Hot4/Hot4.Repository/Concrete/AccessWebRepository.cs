using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;

namespace Hot4.Repository.Concrete
{
    public class AccessWebRepository : RepositoryBase<AccessWeb>, IAccessWebRepository
    {
        public AccessWebRepository(HotDbContext context) : base(context) { }
        public async Task<AccessWeb?> GetAccessWebById(long accessId)
        {
            var record = await GetById(accessId);
            if (record != null)
            {
                return record;
            }
            return null;
        }
       public async Task<bool> AddAccessWeb(AccessWeb accessWeb)
        {
            accessWeb.SalesPassword = true;
            await Create(accessWeb);
            await SaveChanges();
            return true;
        }
       public async Task<bool> UpdateAccessWeb(AccessWeb accessWeb)
        {
            Update(accessWeb);
            await SaveChanges();
            return true;

        }
        public async Task<bool> DeleteAccessWeb(AccessWeb accessWeb)
        {
            Delete(accessWeb);
            await SaveChanges();
            return true;
        }
        

    }
}
