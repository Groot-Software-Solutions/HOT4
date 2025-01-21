using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IProductService
    {
        Task<ProductModel?> GetProductById(byte productId);
        Task<bool> AddProduct(ProductModel productModel);
        Task<bool> UpdateProduct(ProductModel productModel);
        Task<bool> DeleteProduct(byte ProductId);
        Task<List<ProductModel>> ListProduct();
    }
}
