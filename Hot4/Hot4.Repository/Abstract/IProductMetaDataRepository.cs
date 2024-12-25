using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IProductMetaDataRepository
    {
        Task<List<ProductMetaDataModel>> ListProductMetaData();
    }
}
