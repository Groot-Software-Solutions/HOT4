using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProductMetaDataTypeRepository
    {
        Task<ProductMetaDataType> GetProductMetaDataTypeById(byte ProductMetaDataTypeId);
        Task<List<ProductMetaDataType>> ListProductMetaDataType();
        Task <bool>AddProductMetaDataType(ProductMetaDataType productMetaDataType);
        Task<bool> UpdateProductMetaDataType(ProductMetaDataType productMetaDataType);
        Task<bool> DeleteProductMetaDataType(ProductMetaDataType productMetaDataType);
    }
}
