using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(HotDbContext context) : base(context) { }
        public async Task<Product?> GetProductById(byte productId)
        {
            return await GetById(productId);
        }

        public async Task<bool> AddProduct(Product product)
        {
            await Create(product);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateProduct(Product product)
        {
            Update(product);
            await SaveChanges();
            return true;
        }

        public async Task<bool> DeleteProduct(Product product)
        {
            Delete(product);
            await SaveChanges();
            return true;
        }
        public async Task<List<Product>> ListProduct()
        {
            return await GetAll().ToListAsync();
        }
    }
}
