using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchRepository
    {
        Task<PinBatchModel> AddPinBatch(PinBatches pinBatches);
        Task<List<PinBatchVsType>> PinBatch_by_batchType(byte pinBatchTypeId);
    }
}
