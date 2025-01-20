using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SelfTopUpStateRepository : RepositoryBase<SelfTopUpState>, ISelfTopUpStateRepository
    {
        public SelfTopUpStateRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddSelfTopUpState(SelfTopUpState selfTopUpState)
        {
            await Create(selfTopUpState);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteSelfTopUpState(SelfTopUpState selfTopUpState)
        {
            Delete(selfTopUpState);
            await SaveChanges();
            return true;
        }

        public Task<SelfTopUpState?> GetSelfTopUpStateById(byte selfTopUpStateId)
        {
            return GetById(selfTopUpStateId);
        }

        public async Task<List<SelfTopUpState>> ListSelfTopUpState()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<bool> UpdateSelfTopUpState(SelfTopUpState selfTopUpState)
        {
            Update(selfTopUpState);
            await SaveChanges();
            return true;
        }
    }
}
