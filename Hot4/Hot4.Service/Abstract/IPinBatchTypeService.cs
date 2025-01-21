using Hot4.ViewModel;

namespace Hot4.Service.Abstract
{
    public interface IPinBatchTypeService
    {
        Task<bool> AddPinBatchType(PinBatchTypeModel pinBatchTypes);
        Task<bool> UpdatePinBatchType(PinBatchTypeModel pinBatchTypes);
        Task<bool> DeletePinBatchType(byte PinBatchTypeId);
        Task<List<PinBatchTypeModel>> ListPinBatchType();
    }
}
