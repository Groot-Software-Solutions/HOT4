using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchTypeRepository
    {
        Task AddPinBatchType(PinBatchTypes pinBatchTypes);
        Task UpdatePinBatchType(PinBatchTypes pinBatchTypes);
        Task DeletePinBatchType(PinBatchTypes pinBatchTypes);
        Task<List<PinBatchTypeModel>> ListPinBatchType();
    }
}
