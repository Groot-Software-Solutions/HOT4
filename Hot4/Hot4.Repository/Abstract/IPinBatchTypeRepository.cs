using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchTypeRepository
    {
        Task<bool> AddPinBatchType(PinBatchTypes pinBatchTypes);
        Task<bool> UpdatePinBatchType(PinBatchTypes pinBatchTypes);
        Task<bool> DeletePinBatchType(PinBatchTypes pinBatchTypes);
        Task<List<PinBatchTypes>> ListPinBatchType();
        Task<PinBatchTypes?> GetPinBatchTypeById(byte pinBatchTypeId);
    }
}
