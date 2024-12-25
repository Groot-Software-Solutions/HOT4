using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class ProductMetaDataTypeRepository : RepositoryBase<ProductMetaDataType>, IProductMetaDataTypeRepository
    {
        public ProductMetaDataTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<ProductMetaDataTypeModel>> ListProductMetaDataType()
        {
            return await GetAll()
                .Select(d => new ProductMetaDataTypeModel
                {
                    Description = d.Description,
                    Name = d.Name,
                    ProductMetaDataTypeId = d.ProductMetaDataTypeId,
                }).ToListAsync();

        }
    }
}
