using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class CommonRepository : ICommonRepository
    {
        private HotDbContext _context { get; set; }
        public CommonRepository(HotDbContext context)
        {
            _context = context;
        }
        public async Task<float> GetPrePaidStockBalance(int brandId)
        {
            return (float)await (from r in _context.Recharge
                                 where r.BrandId == brandId && r.StateId == (int)SmsState.Success
                                 join rp in _context.RechargePrepaid
                                 on r.RechargeId equals rp.RechargeId
                                 where rp.FinalWallet != 0
                                 orderby r.RechargeId descending
                                 select rp.FinalWallet
                           ).Take(2).DefaultIfEmpty(0).MinAsync();
        }
    }
}
