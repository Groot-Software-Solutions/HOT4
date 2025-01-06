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

        public async Task<ProductModel?> GetProductById(int productId)
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

            return null;
        }

        public async Task AddProduct(Product product)
        {
            await Create(product);
            await SaveChanges();
        }
        public async Task UpdateProduct(Product product)
        {
            Update(product);
            await SaveChanges();
        }

        public async Task DeleteProduct(Product product)
        {
            Delete(product);
            await SaveChanges();
        }
        public async Task<List<ProductModel>> ListProduct()
        {
            return await GetAll().Select(d => new ProductModel
            {
                BrandId = d.BrandId,
                Name = d.Name,
                ProductId = d.ProductId,
                WalletTypeId = d.WalletTypeId,
                ProductStateId = d.ProductStateId,
            }).ToListAsync();
        }
    }
}
