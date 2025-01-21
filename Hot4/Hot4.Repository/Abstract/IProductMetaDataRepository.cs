using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProductMetaDataRepository
    {
        Task<ProductMetaData> GetProductMetaDataById(int ProductMetaId);
        Task<List<ProductMetaData>> ListProductMetaData();
        Task<bool> AddProductMetaData(ProductMetaData productMetaData);
        Task<bool> UpdateProductMetaData(ProductMetaData productMetaData);
        Task<bool> DeleteProductMetaData(ProductMetaData productMetaData);
    }
}
