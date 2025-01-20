using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProductFieldRepository : RepositoryBase<ProductField>, IProductFieldRepository
    {
        public ProductFieldRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddProductField(ProductField productField)
        {
            await Create(productField);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteProductField(ProductField productField)
        {
            Delete(productField);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateProductField(ProductField productField)
        {
            Update(productField);
            await SaveChanges();
            return true;
        }
        public async Task<List<ProductFieldModel>> ListProductField()
        {
            return await GetAll()
                .Select(d => new ProductFieldModel
                {
                    BrandFieldId = d.BrandFieldId,
                    DataType = d.DataType,
                    Description = d.Description,
                    FieldName = d.FieldName,
                    ProductId = d.ProductId
                }).ToListAsync();
        }

        public async Task<ProductField?> GetProductFieldById(int BrandFieldId)
        {
            return await GetById(BrandFieldId);
        }
    }
}
