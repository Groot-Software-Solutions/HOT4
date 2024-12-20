using Hot4.Core.DataViewModels;
using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IBankTrxBatchRepository
    {
        Task<BankTrxBatch> AddBatch(BankTrxBatch bankTrxBatch);
        Task<List<BankBatchModel>> GetBatchByBank(byte bankId);
        Task<long?> GetCurrentBatchIdByBank(byte bankId);
        Task<long?> GetCurrentBatchIdByBankAndRef(byte bankId, string batchReference);
        Task<BankTrxBatch?> GetCurrentBatch(byte bankId, string batchReference, string lastUser);

    }
}
