using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class HotTypeRepository : RepositoryBase<TblHotType>, IHotTypeRepository
    {
        public HotTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<TblHotType>> ListHotType()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<TblHotType?> GetHotType(int hotTypeId)
        {
            return await GetById(hotTypeId);
        }
    }
}
