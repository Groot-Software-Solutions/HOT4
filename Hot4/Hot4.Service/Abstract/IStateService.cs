using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IStateService
    {
        Task<StateModel?> GetStateById(byte stateId);
        Task<List<StateModel>> ListState();
        Task<bool> AddState(StateModel state);
        Task<bool> UpdateState(StateModel state);
        Task<bool> DeleteState(byte stateId);
    }
}
