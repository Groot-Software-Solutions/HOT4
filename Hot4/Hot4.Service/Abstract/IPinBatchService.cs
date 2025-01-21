using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IPinBatchService
    {

        Task<bool> AddPinBatch(PinBatchRecord pinBatches);
        Task<bool> UpdatePinBatch(PinBatchRecord pinBatches);
        Task<bool> DeletePinBatch(PinBatchRecord pinBatches);
        Task<List<PinBatchModel>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId);
        Task<PinBatchModel?> GetPinBatchById(long pinBatchId);
    }
}
