using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class StateRepository : RepositoryBase<States>, IStateRepository
    {
        public StateRepository(HotDbContext context) : base(context) { }

        public async Task AddState(States state)
        {
            await Create(state);
            await SaveChanges();
        }
        public async Task DeleteState(States state)
        {
            Delete(state);
            await SaveChanges();
        }
        public async Task<StateModel?> GetStateById(byte stateId)
        {
            var result = await GetById(stateId);
            if (result != null)
            {
                return new StateModel
                {
                    StateId = result.StateId,
                    StateName = result.State
                };
            }
            return null;
        }
        public async Task<List<StateModel>> ListState()
        {
            return await GetAll().Select(d => new StateModel
            {
                StateId = d.StateId,
                StateName = d.State
            }).ToListAsync();
        }
        public async Task UpdateState(States state)
        {
            Update(state);
            await SaveChanges();
        }
    }
}
