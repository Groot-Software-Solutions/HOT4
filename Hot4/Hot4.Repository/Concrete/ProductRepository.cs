using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;

namespace Hot4.Repository.Concrete
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(HotDbContext context) : base(context) { }

        public async Task<ProductModel?> GetProduct(int productId)
        {
            var result = await GetById(productId);
            if (result != null)
            {
                return new ProductModel
                {
                    BrandId = result.BrandId,
                    Name = result.Name,
                    ProductId = result.ProductId,
                    ProductStateId = result.ProductStateId,
                    WalletTypeId = result.WalletTypeId,
                };
            }
            else
            {
                return null;
            }
        }

        public async Task AddProduct(Product product)
        {
            await Create(product);
            await SaveChanges();
        }
        public async Task UpdateProduct(Product product)
        {

            await Update(product);
            await SaveChanges();

        }

        public async Task DeleteProduct(Product product)
        {
            await Delete(product);
            await SaveChanges();
        }
    }
}
