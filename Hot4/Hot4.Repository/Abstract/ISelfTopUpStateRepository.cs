using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISelfTopUpStateRepository
    {
        Task<bool> AddSelfTopUpState(SelfTopUpState selfTopUpState);
        Task<bool> UpdateSelfTopUpState(SelfTopUpState selfTopUpState);
        Task<bool> DeleteSelfTopUpState(SelfTopUpState selfTopUpState);
        Task<SelfTopUpState?> GetSelfTopUpStateById(byte selfTopUpStateId);
        Task<List<SelfTopUpState>> ListSelfTopUpState();
    }
}
