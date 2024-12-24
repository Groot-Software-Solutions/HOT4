using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchTypeRepository
    {
        Task<List<PinBatchTypeModel>> ListPinBatchType();
    }
}
