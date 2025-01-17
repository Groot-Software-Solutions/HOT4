using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PinBatchRepository : RepositoryBase<PinBatches>, IPinBatchRepository
    {
        public PinBatchRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddPinBatch(PinBatches pinBatch)
        {
            var pinBatchExist = await _context.PinBatch.FirstOrDefaultAsync(d => d.PinBatch == pinBatch.PinBatch
                                 && d.PinBatchTypeId == pinBatch.PinBatchTypeId);
            if (pinBatchExist == null)
            {
                pinBatch.BatchDate = DateTime.Now;
                await Create(pinBatch);
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdatePinBatch(PinBatches pinBatches)
        {
            Update(pinBatches);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeletePinBatch(PinBatches pinBatches)
        {
            Delete(pinBatches);
            await SaveChanges();
            return true;
        }
        public async Task<List<PinBatches>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId)
        {
            return await GetByCondition(d => d.PinBatchTypeId == pinBatchTypeId)
                        .Include(d => d.PinBatchType)
                        .OrderBy(d => d.BatchDate)
                        .ToListAsync();
        }
        public async Task<PinBatches?> GetPinBatchById(long pinBatchId)
        {
            return await _context.PinBatch.Include(d => d.PinBatchType).FirstOrDefaultAsync(d => d.PinBatchId == pinBatchId);
        }
    }
}
