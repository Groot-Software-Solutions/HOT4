using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface ISelfTopUpRepository
    {
        Task<bool> AddSelfTopUp(SelfTopUp selfTopUp);
        Task<bool> UpdateSelfTopUp(SelfTopUp selfTopUp);
        Task<SelfTopUp?> GetSelfTopUpById(long selfTopUpId);
        Task<List<SelfTopUp>> GetSelfTopUpByStateId(byte selfTopUpStateId);
        Task<List<SelfTopUp>> GetSelfTopUpByBankTrxId(long bankTrxId);
        Task<bool> DeleteSelfTopUp(SelfTopUp selfTopUp);
    }
}
