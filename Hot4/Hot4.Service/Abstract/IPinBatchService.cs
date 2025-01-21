using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IPinBatchService
    {

        Task<bool> AddPinBatch(PinBatchToDo pinBatches);
        Task<bool> UpdatePinBatch(PinBatchToDo pinBatches);
        Task<bool> DeletePinBatch(long PinBatchId);
        Task<List<PinBatchModel>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId);
        Task<PinBatchModel?> GetPinBatchById(long pinBatchId);
    }
}
