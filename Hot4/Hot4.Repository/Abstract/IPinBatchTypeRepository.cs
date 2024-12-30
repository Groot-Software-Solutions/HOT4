using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchTypeRepository
    {
        Task<List<PinBatchTypeModel>> ListPinBatchType();
    }
}
