using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IProductMetaDataTypeService
    {
        Task<ProductMetaDataTypeModel> GetProductMetaDataTypeById(byte ProductMetaDataTypeId);
        Task<List<ProductMetaDataTypeModel>> ListProductMetaDataType();
        Task<bool> AddProductMetaDataType(ProductMetaDataTypeModel productMetaDataTypeModel);
        Task<bool> UpdateProductMetaDataType(ProductMetaDataTypeModel productMetaDataTypeModel);
        Task<bool> DeleteProductMetaDataType(ProductMetaDataTypeModel productMetaDataTypeModel);
    }
}
