using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ISelfTopUpStateService
    {
        Task<bool> AddSelfTopUpState(SelfTopUpStateModel selfTopUpState);
        Task<bool> UpdateSelfTopUpState(SelfTopUpStateModel selfTopUpState);
        Task<bool> DeleteSelfTopUpState(byte selfTopUpStateId);
        Task<SelfTopUpStateModel?> GetSelfTopUpStateById(byte selfTopUpStateId);
        Task<List<SelfTopUpStateModel>> ListSelfTopUpState();
    }
}
