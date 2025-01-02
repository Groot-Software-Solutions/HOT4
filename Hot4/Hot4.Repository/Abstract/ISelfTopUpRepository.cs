using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface ISelfTopUpRepository
    {
        Task<long> AddSelfTopUp(SelfTopUp selfTopUp);
        Task UpdateSelfTopUp(SelfTopUp selfTopUp);
        Task<List<SelfTopUpModel>> GetSelfTopUpByStateId(byte selfTopUpStateId);
        Task<List<SelfTopUpModel>> GetSelfTopUpByBankTrxId(long bankTrxId);
        Task DeleteSelfTopUp(SelfTopUp selfTopUp);
    }
}
