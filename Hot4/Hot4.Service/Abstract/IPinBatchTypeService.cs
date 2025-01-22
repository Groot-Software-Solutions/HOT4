using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IPinBatchTypeService
    {
        Task<PinBatchTypeModel> GetPinBatchById(byte pinBatchTypeId);
        Task<bool> AddPinBatchType(PinBatchTypeModel pinBatchTypes);
        Task<bool> UpdatePinBatchType(PinBatchTypeModel pinBatchTypes);
        Task<bool> DeletePinBatchType(byte pinBatchTypeId);
        Task<List<PinBatchTypeModel>> ListPinBatchType();
    }
}
