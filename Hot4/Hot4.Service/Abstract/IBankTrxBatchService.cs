using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IBankTrxBatchService
    {
        Task<bool> AddBatch(BankTrxBatchToDo bankTrxBatch);
        Task<bool> UpdateBatch(BankTrxBatchToDo bankTrxBatch);
        Task<bool> DeleteBatch(long batchId);
        Task<BankBatchModel?> GetBatchById(long batchId);
        Task<List<BankBatchModel>> GetBatchByBankId(byte bankId);
        Task<long?> GetCurrentBatchByBankIdAndRefId(byte bankId, string batchRef = null);
        Task<BankBatchModel?> GetCurrentBatch(byte bankId, string batchReference, string lastUser);
    }
}
