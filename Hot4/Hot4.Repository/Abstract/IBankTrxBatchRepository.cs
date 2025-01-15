
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxBatchRepository
    {
        Task<long> AddBatch(BankTrxBatch bankTrxBatch);
        Task UpdateBatch(BankTrxBatch bankTrxBatch);
        Task DeleteBatch(BankTrxBatch bankTrxBatch);
        Task<BankTrxBatch?> GetBatchById(long batchId);
        Task<List<BankTrxBatch>> GetBatchByBankId(byte bankId);
        Task<long?> GetCurrentBatchByBankIdAndRefId(byte bankId, string batchRef = null);
        Task<BankTrxBatch?> GetCurrentBatch(byte bankId, string batchReference, string lastUser);

    }
}
