using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISelfTopUpRepository
    {
        Task<bool> AddSelfTopUp(SelfTopUp selfTopUp);
        Task<bool> UpdateSelfTopUp(SelfTopUp selfTopUp);
        Task<SelfTopUp?> GetSelfTopUpById(long selfTopUpId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByStateId(byte selfTopUpStateId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByBankTrxId(long bankTrxId);
        Task<bool> DeleteSelfTopUp(SelfTopUp selfTopUp);
    }
}
