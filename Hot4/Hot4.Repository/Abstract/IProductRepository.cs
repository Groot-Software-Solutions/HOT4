using Hot4.DataModel.Models;

namespace Hot4.Repository.Abstract
{
    public interface IProductRepository
    {
        Task<TblProduct?> GetProduct(int productId);
        Task<int> AddProduct(TblProduct product);
        Task UpdateProduct(TblProduct product);
        Task DeleteProduct(int productId);



    }
}
