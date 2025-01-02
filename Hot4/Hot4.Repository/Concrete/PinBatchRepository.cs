using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class PinBatchRepository : RepositoryBase<PinBatches>, IPinBatchRepository
    {
        public PinBatchRepository(HotDbContext context) : base(context) { }
        public async Task<PinBatchModel> AddPinBatch(PinBatches pinBatch)
        {
            var pinBatchExist = await _context.PinBatch.FirstOrDefaultAsync(d => d.PinBatch == pinBatch.PinBatch
                                 && d.PinBatchTypeId == pinBatch.PinBatchTypeId);
            if (pinBatchExist == null)
            {
                pinBatch.BatchDate = DateTime.Now;
                await Create(pinBatch);
                await SaveChanges();

                return new PinBatchModel
                {
                    PinBatchId = pinBatch.PinBatchId,
                    BatchDate = pinBatch.BatchDate,
                    PinBatch = pinBatch.PinBatch
                };
            }
            else
            {
                return new PinBatchModel
                {
                    PinBatchId = pinBatchExist.PinBatchId,
                    BatchDate = pinBatchExist.BatchDate,
                    PinBatch = pinBatchExist.PinBatch
                };
            }
        }
        public async Task UpdatePinBatch(PinBatches pinBatches)
        {
            await Update(pinBatches);
            await SaveChanges();
        }
        public async Task DeletePinBatch(PinBatches pinBatches)
        {
            await Delete(pinBatches);
            await SaveChanges();
        }
        public async Task<List<PinBatchVsType>> GetPinBatchByPinBatchTypeId(byte pinBatchTypeId)
        {
            return await GetByCondition(d => d.PinBatchTypeId == pinBatchTypeId)
                        .Include(d => d.PinBatchType)
                        .OrderBy(d => d.BatchDate)
                        .Select(d => new PinBatchVsType
                        {
                            BatchDate = d.BatchDate,
                            PinBatch = d.PinBatch,
                            PinBatchId = d.PinBatchId,
                            PinBatchType = d.PinBatchType.PinBatchType,
                            PinBatchTypeId = d.PinBatchTypeId
                        }).ToListAsync();
        }
        public async Task<PinBatchVsType?> GetPinBatchById(long pinBatchId)
        {
            var result = await _context.PinBatch.Include(d => d.PinBatchType).FirstOrDefaultAsync(d => d.PinBatchId == pinBatchId);
            if (result != null)
            {

                return new PinBatchVsType
                {
                    BatchDate = result.BatchDate,
                    PinBatch = result.PinBatch,
                    PinBatchId = result.PinBatchId,
                    PinBatchType = result.PinBatchType.PinBatchType,
                    PinBatchTypeId = result.PinBatchTypeId
                };
            }
            return null;
        }
    }
}
