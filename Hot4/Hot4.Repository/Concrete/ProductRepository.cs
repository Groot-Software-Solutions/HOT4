using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;

namespace Hot4.Repository.Concrete
{
    public class ProductRepository : RepositoryBase<TblProduct>, IProductRepository
    {
        public ProductRepository(HotDbContext context) : base(context) { }

        public async Task<TblProduct?> GetProduct(int productId)
        {
            return await GetById(productId);
        }

        public async Task<int> AddProduct(TblProduct product)
        {
            await Create(product);
            await SaveChanges();
            return product.ProductId;
        }
        public async Task UpdateProduct(TblProduct product)
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
