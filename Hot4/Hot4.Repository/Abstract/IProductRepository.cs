using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IProductRepository
    {
        Task<Product?> GetProduct(int productId);
        Task<int> AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int productId);



    }
}
