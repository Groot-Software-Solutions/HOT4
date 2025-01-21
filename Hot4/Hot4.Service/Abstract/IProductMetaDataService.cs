using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IProductMetaDataService
    {
        Task<ProductMetaDataModel> GetProductMetaDataById(int ProductMetaId);
        Task<List<ProductMetaDataModel>> ListProductMetaData();
        Task<bool> AddProductMetaData(ProductMetaDataModel productMetaDataModel);
        Task<bool> UpdateProductMetaData(ProductMetaDataModel productMetaDataModel);
        Task<bool> DeleteProductMetaData(int ProductMetaId);
    }
}
