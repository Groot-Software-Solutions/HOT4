using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class HotTypeRepository : RepositoryBase<HotTypes>, IHotTypeRepository
    {
        public HotTypeRepository(HotDbContext context) : base(context) { }
        public async Task<List<HotTypeModel>> ListHotType()
        {
            return await (from hotyp in _context.HotType
                          join hotypCod in _context.HotTypeCode on hotyp.HotTypeId equals hotypCod.HotTypeId
                          select new HotTypeModel
                          {
                              HotTypeId = hotyp.HotTypeId,
                              HotType = hotyp.HotType,
                              SplitCount = hotyp.SplitCount ?? 0,
                              HotTypeCodeId = hotypCod.HotTypeCodeId,
                              TypeCode = hotypCod.TypeCode,
                          }).ToListAsync();
        }

        public async Task<byte?> GetHotTypeIdentity(string typeCode, byte splitCount)
        {
            return await (from ht in _context.HotType
                          where (ht.SplitCount ?? splitCount) == splitCount
                          join htcode in _context.HotTypeCode on ht.HotTypeId equals htcode.HotTypeId
                          where htcode.TypeCode.ToUpper() == typeCode.ToUpper()
                          select ht.HotTypeId).LastOrDefaultAsync();
        }
    }
}
