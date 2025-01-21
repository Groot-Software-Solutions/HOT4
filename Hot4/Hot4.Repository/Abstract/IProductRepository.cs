using Hot4.DataModel.Models;
using Hot4.ViewModel;

namespace Hot4.Repository.Abstract
{
    public interface IProductRepository
    {
        Task<Product?> GetProductById(byte productId);
        Task<bool> AddProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(Product product);
        Task<List<Product>> ListProduct();
    }
}
