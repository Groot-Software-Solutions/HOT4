using Hot4.DataModel.Models;
using Hot4.ViewModel.ApiModels;

namespace Hot4.Repository.Abstract
{
    public interface IProductMetaDataTypeRepository
    {
        Task<List<ProductMetaDataTypeModel>> ListProductMetaDataType();
        Task AddProductMetaDataType(ProductMetaDataType productMetaDataType);
        Task UpdateProductMetaDataType(ProductMetaDataType productMetaDataType);
        Task DeleteProductMetaDataType(ProductMetaDataType productMetaDataType);
    }
}
