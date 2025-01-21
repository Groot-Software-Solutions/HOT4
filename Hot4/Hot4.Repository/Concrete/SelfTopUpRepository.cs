using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class SelfTopUpRepository : RepositoryBase<SelfTopUp>, ISelfTopUpRepository
    {
        public SelfTopUpRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddSelfTopUp(SelfTopUp selfTopUp)
        {
            await Create(selfTopUp);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateSelfTopUp(SelfTopUp selfTopUp)
        {
            Update(selfTopUp);
            await SaveChanges();
            return true;
        }
        public async Task<SelfTopUp?> GetSelfTopUpById(long selfTopUpId)
        {
            return await _context.SelfTopUp.Where(d => d.SelfTopUpId == selfTopUpId)
                             .Include(d => d.Access).Include(d => d.Brand).Include(d => d.SelfTopUpState)
                             .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteSelfTopUp(SelfTopUp selfTopUp)
        {
            Delete(selfTopUp);
            await SaveChanges();
            return true;
        }
        public async Task<List<SelfTopUp>> GetSelfTopUpByBankTrxId(long bankTrxId)
        {
            return await _context.SelfTopUp.Include(d => d.Access).Include(d => d.Brand).Include(d => d.SelfTopUpState)
                .Where(d => d.BankTrxId == bankTrxId).OrderBy(d => d.SelfTopUpId).ToListAsync();
        }
        public async Task<List<SelfTopUp>> GetSelfTopUpByStateId(byte selfTopUpStateId)
        {
            return await _context.SelfTopUp.Include(d => d.Access).Include(d => d.Brand).Include(d => d.SelfTopUpState)
                .Where(d => d.StateId == selfTopUpStateId).OrderBy(d => d.SelfTopUpId).ToListAsync();
        }


    }
}
