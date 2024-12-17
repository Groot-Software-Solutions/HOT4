using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class HotTypeRepository : RepositoryBase<HotTypes>, IHotTypeRepository
    {
        public HotTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<HotTypes>> ListHotType()
        {
            return await GetAll().ToListAsync();
        }

        public async Task<HotTypes?> GetHotType(int hotTypeId)
        {
            return await GetById(hotTypeId);
        }
    }
}
