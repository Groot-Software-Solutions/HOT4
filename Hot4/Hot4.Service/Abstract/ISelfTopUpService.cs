using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ISelfTopUpService
    {
        Task<bool> AddSelfTopUp(SelfTopUpToDo selfTopUp);
        Task<bool> UpdateSelfTopUp(SelfTopUpToDo selfTopUp);
        Task<SelfTopUpModel?> GetSelfTopUpById(long selfTopUpId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByStateId(byte selfTopUpStateId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByBankTrxId(long bankTrxId);
        Task<bool> DeleteSelfTopUp(long selfTopUpId);
    }
}
