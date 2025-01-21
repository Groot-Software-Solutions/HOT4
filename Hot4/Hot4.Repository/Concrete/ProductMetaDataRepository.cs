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

        public async Task<bool> AddProductMetaData(ProductMetaData productMetaData)
        {
            await Create(productMetaData);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteProductMetaData(ProductMetaData productMetaData)
        {
            Delete(productMetaData);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateProductMetaData(ProductMetaData productMetaData)
        {
            Update(productMetaData);
            await SaveChanges();
            return true;
        }
        public async Task<List<ProductMetaData>> ListProductMetaData()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<ProductMetaData?> GetProductMetaDataById(int ProductMetaId)
        {
            return await GetById(ProductMetaId);
        }
    }
}
