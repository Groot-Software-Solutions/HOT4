using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProductMetaDataTypeRepository : RepositoryBase<ProductMetaDataType>, IProductMetaDataTypeRepository
    {
        public ProductMetaDataTypeRepository(HotDbContext context) : base(context) { }

        public async Task<bool> AddProductMetaDataType(ProductMetaDataType productMetaDataType)
        {
            await Create(productMetaDataType);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteProductMetaDataType(ProductMetaDataType productMetaDataType)
        {
            Delete(productMetaDataType);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateProductMetaDataType(ProductMetaDataType productMetaDataType)
        {
            Update(productMetaDataType);
            await SaveChanges();
            return true;
        }
        public async Task<List<ProductMetaDataType>> ListProductMetaDataType()
        {
            return await GetAll().ToListAsync();

        }
        public async Task<ProductMetaDataType?> GetProductMetaDataTypeById(byte ProductMetaDataTypeId)
        {
            return await GetById(ProductMetaDataTypeId);
        }
    }
}
