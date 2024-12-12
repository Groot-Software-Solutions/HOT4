using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IStateRepository
    {
        Task<List<States>> ListState();
        Task<States?> GetState(byte stateId);
    }
}
