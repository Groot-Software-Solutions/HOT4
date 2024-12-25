using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IProductMetaDataRepository
    {
        Task<List<ProductMetaDataModel>> ListProductMetaData();
        Task AddProductMetaData(ProductMetaData productMetaData);
        Task UpdateProductMetaData(ProductMetaData productMetaData);
        Task DeleteProductMetaData(ProductMetaData productMetaData);
    }
}
