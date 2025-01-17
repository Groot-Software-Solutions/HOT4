using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class StateRepository : RepositoryBase<States>, IStateRepository
    {
        public StateRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddState(States state)
        {
            await Create(state);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteState(States state)
        {
            Delete(state);
            await SaveChanges();
            return true;
        }
        public async Task<States?> GetStateById(byte stateId)
        {
            return await GetById(stateId);
        }
        public async Task<List<States>> ListState()
        {
            return await GetAll().ToListAsync();
        }
        public async Task<bool> UpdateState(States state)
        {
            Update(state);
            await SaveChanges();
            return true;
        }
    }
}
