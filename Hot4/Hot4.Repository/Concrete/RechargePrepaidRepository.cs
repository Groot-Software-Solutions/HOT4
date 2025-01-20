//using Hot4.Core.DataViewModels;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class RechargePrepaidRepository : RepositoryBase<RechargePrepaid>, IRechargePrepaidRepository
    {
        public RechargePrepaidRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddRechargePrepaid(RechargePrepaid rechargeprepaid)
        {
            await Create(rechargeprepaid);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateRechargePrepaid(RechargePrepaid rechargePrepaid)
        {
            Update(rechargePrepaid);
            await SaveChanges();
            return true;
        }
        public async Task<RechargePrepaid?> GetRechargePrepaidById(long rechargeId)
        {
            return await GetById(rechargeId);
        }

    }
}
