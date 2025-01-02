using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IStateRepository
    {
        Task<StateModel?> GetStateById(byte stateId);
        Task<List<StateModel>> ListState();
        Task AddState(States state);
        Task UpdateState(States state);
        Task DeleteState(States state);
    }
}
