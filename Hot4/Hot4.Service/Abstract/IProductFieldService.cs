using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IProductFieldService 
    {
        Task<ProductFieldModel> GetProductFieldById(int BrandFieldId);
        Task<List<ProductFieldModel>> ListProductField();
        Task<bool> AddProductField(ProductFieldModel productFieldModel);
        Task<bool> UpdateProductField(ProductFieldModel productFieldModel);
        Task<bool> DeleteProductField(ProductFieldModel productFieldModel);
    }
}
