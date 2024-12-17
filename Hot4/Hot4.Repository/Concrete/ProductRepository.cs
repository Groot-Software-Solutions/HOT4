using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(HotDbContext context) : base(context) { }

        public async Task<Product?> GetProduct(int productId)
        {
            return await GetById(productId);
        }

        public async Task<int> AddProduct(Product product)
        {
            await Create(product);
            await SaveChanges();
            return product.ProductId;
        }
        public async Task UpdateProduct(Product product)
        {

            await Update(product);
            await SaveChanges();

        }
        public async Task DeleteProduct(int productId)
        {
            var existing = await GetById(productId);
            if (existing != null)
            {
                await Delete(existing);
                await SaveChanges();
            }
        }


    }
}
