using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SelfTopUpStateRepository : RepositoryBase<SelfTopUpState>, ISelfTopUpStateRepository
    {
        public SelfTopUpStateRepository(HotDbContext context) : base(context) { }
        public async Task<long> AddSelfTopUpState(SelfTopUpState selfTopUpState)
        {
            await Create(selfTopUpState);
            await SaveChanges();
            return selfTopUpState.SelfTopUpStateId;
        }
        public async Task DeleteSelfTopUpState(SelfTopUpState selfTopUpState)
        {
            Delete(selfTopUpState);
            await SaveChanges();
        }
        public async Task<List<SelfTopUpStateModel>> ListSelfTopUpState()
        {
            return await GetAll().Select(d => new SelfTopUpStateModel
            {
                SelfTopUpStateId = d.SelfTopUpStateId,
                SelfTopUpStateName = d.SelfTopUpStateName,
            }).ToListAsync();
        }
        public async Task UpdateSelfTopUpState(SelfTopUpState selfTopUpState)
        {
            Update(selfTopUpState);
            await SaveChanges();
        }
    }
}
