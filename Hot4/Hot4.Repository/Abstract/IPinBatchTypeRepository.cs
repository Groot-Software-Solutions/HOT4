using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IPinBatchTypeRepository
    {
        public Task<List<PinBatchTypeModel>> ListPinBatchType();
    }
}
