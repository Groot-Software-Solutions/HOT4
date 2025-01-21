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
            return await _context.HotType.Include(d => d.HotTypeCodes).ToListAsync();
        }
        public async Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount)
        {
            return await (from ht in _context.HotType
                          where (ht.SplitCount ?? splitCount) == splitCount
                          join htcode in _context.HotTypeCode on ht.HotTypeId equals htcode.HotTypeId
                          where htcode.TypeCode.ToUpper() == typeCode.ToUpper()
                          select ht.HotTypeId).LastOrDefaultAsync();
        }

        public async Task<bool> AddHotType(HotTypes hotTypes)
        {
            await Create(hotTypes);
            await SaveChanges();
            return true;
        }

        public async Task<bool> UpdateHotType(HotTypes hotTypes)
        {
            Update(hotTypes);
            await SaveChanges();
            return true;
        }

        public async Task<bool> DeleteHotType(HotTypes hotTypes)
        {
            Delete(hotTypes);
            await SaveChanges();
            return true;
        }

        public async Task<HotTypes?> GetHotTypeById(byte HotTypeId)
        {
            return await GetById(HotTypeId);
        }
    }
}
