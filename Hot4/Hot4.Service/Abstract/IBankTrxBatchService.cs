using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankTrxBatchService
    {
        Task<long> AddBatch(BankTrxBatchToDo bankTrxBatch);
        Task UpdateBatch(BankTrxBatchToDo bankTrxBatch);
        Task DeleteBatch(long batchId);
        Task<BankBatchModel?> GetBatchById(long batchId);
        Task<List<BankBatchModel>> GetBatchByBankId(byte bankId);
        Task<long?> GetCurrentBatchByBankIdAndRefId(byte bankId, string batchRef = null);
        Task<BankBatchModel?> GetCurrentBatch(byte bankId, string batchReference, string lastUser);
    }
}
