using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IStateRepository
    {
        Task<States?> GetStateById(byte stateId);
        Task<List<States>> ListState();
        Task<bool> AddState(States state);
        Task<bool> UpdateState(States state);
        Task<bool> DeleteState(States state);
    }
}
