using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProductMetaDataRepository : RepositoryBase<ProductMetaData>, IProductMetaDataRepository
    {
        public ProductMetaDataRepository(HotDbContext context) : base(context) { }

        public async Task AddProductMetaData(ProductMetaData productMetaData)
        {
            await Create(productMetaData);
            await SaveChanges();
        }
        public async Task DeleteProductMetaData(ProductMetaData productMetaData)
        {
            await Delete(productMetaData);
            await SaveChanges();
        }
        public async Task UpdateProductMetaData(ProductMetaData productMetaData)
        {
            await Update(productMetaData);
            await SaveChanges();
        }
        public async Task<List<ProductMetaDataModel>> ListProductMetaData()
        {
            return await GetAll()
                 .Select(d => new ProductMetaDataModel
                 {
                     BrandMetaId = d.ProductMetaId,
                     ProductMetaId = d.ProductMetaId,
                     Data = d.Data,
                     ProductId = d.ProductId,
                     ProductMetaDataTypeId = d.ProductMetaDataTypeId,
                 }).ToListAsync();
        }

    }
}
