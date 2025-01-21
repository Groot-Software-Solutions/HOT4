using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface ISelfTopUpService
    {
        Task<bool> AddSelfTopUp(SelfTopUpRecord selfTopUp);
        Task<bool> UpdateSelfTopUp(SelfTopUpRecord selfTopUp);
        Task<SelfTopUpModel?> GetSelfTopUpById(long selfTopUpId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByStateId(byte selfTopUpStateId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByBankTrxId(long bankTrxId);
        Task<bool> DeleteSelfTopUp(long selfTopUpId);
    }
}
