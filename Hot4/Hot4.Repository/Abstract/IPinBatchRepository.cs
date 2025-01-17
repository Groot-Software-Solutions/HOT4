using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchRepository
    {
        Task<bool> AddPinBatch(PinBatches pinBatches);
        Task<bool> UpdatePinBatch(PinBatches pinBatches);
        Task<bool> DeletePinBatch(PinBatches pinBatches);
        Task<List<PinBatches>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId);
        Task<PinBatches?> GetPinBatchById(long pinBatchId);
    }
}
