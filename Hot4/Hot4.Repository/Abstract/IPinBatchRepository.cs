using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchRepository
    {
        public Task<PinBatchModel> AddPinBatch(PinBatches pinBatches);
        public Task<List<PinBatchVsType>> PinBatch_by_batchType(byte pinBatchTypeId);
    }
}
