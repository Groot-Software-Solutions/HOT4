using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchRepository
    {
        Task<PinBatchModel> AddPinBatch(PinBatches pinBatches);
        Task UpdatePinBatch(PinBatches pinBatches);
        Task DeletePinBatch(PinBatches pinBatches);
        Task<List<PinBatchVsType>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId);
        Task<PinBatchVsType?> GetPinBatchById(long pinBatchId);
    }
}
