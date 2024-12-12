using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IStateRepository
    {
        Task<List<TblState>> ListState();
        Task<TblState?> GetState(byte stateId);
    }
}
