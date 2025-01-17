using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxBatchRepository : RepositoryBase<BankTrxBatch>, IBankTrxBatchRepository
    {
        public BankTrxBatchRepository(HotDbContext context) : base(context) { }
        public async Task<bool> AddBatch(BankTrxBatch bankTrxBatch)
        {
            await Create(bankTrxBatch);
            await SaveChanges();
            return true;
        }
        public async Task<bool> UpdateBatch(BankTrxBatch bankTrxBatch)
        {
            Update(bankTrxBatch);
            await SaveChanges();
            return true;
        }
        public async Task<bool> DeleteBatch(BankTrxBatch bankTrxBatch)
        {
            Delete(bankTrxBatch);
            await SaveChanges();
            return true;
        }
        public async Task<BankTrxBatch?> GetBatchById(long batchId)
        {
            return await _context.BankTrxBatch.Include(d => d.Bank).FirstOrDefaultAsync(d => d.BankTrxBatchId == batchId);
        }
        public async Task<List<BankTrxBatch>> GetBatchByBankId(byte bankId)
        {
            return await GetByCondition(d => d.BankId == bankId)
                .Include(d => d.Bank)
                .OrderBy(d => d.BatchDate).ToListAsync();
        }
        public async Task<long?> GetCurrentBatchByBankIdAndRefId(byte bankId, string batchRef = null)
        {
            var dateNow = DateTime.Now.Date;
            var startOfDay = dateNow;
            var endOfDay = dateNow.AddDays(1).AddTicks(-1);

            if (string.IsNullOrEmpty(batchRef))
            {
                var bankTrxBatchId = await _context.BankTrxBatch.OrderByDescending(d => d.BankTrxBatchId)
                    .FirstOrDefaultAsync(d => d.BankId == bankId
                && d.BatchDate >= startOfDay
                && d.BatchDate <= endOfDay);

                return bankTrxBatchId?.BankTrxBatchId;
            }
            else
            {
                var bankTrxBatchId = await _context.BankTrxBatch.OrderByDescending(d => d.BankTrxBatchId)
                    .FirstOrDefaultAsync(d => d.BankId == bankId
                && d.BatchReference == batchRef
                && d.BatchDate >= startOfDay && d.BatchDate <= endOfDay);

                return bankTrxBatchId?.BankTrxBatchId;
            }
        }

        public async Task<BankTrxBatch?> GetCurrentBatch(byte bankId, string batchReference, string lastUser)
        {
            long? bankTrxBatchId = null;
            if (bankId == (int)BankName.EcoMerchant)
            {
                bankTrxBatchId = await GetCurrentBatchByBankIdAndRefId(bankId);
            }
            else
            {
                bankTrxBatchId = await GetCurrentBatchByBankIdAndRefId(bankId, batchReference);
            }
            if (bankTrxBatchId == null || bankTrxBatchId == 0)
            {
                var model = new BankTrxBatch()
                {
                    BankId = bankId,
                    BatchReference = batchReference,
                    LastUser = lastUser,
                    BatchDate = DateTime.Now
                };
                await AddBatch(model);
                bankTrxBatchId = model.BankTrxBatchId;
            }
            if (bankTrxBatchId != null)
            {
                return await GetBatchById(bankTrxBatchId.Value);
            }
            return null;
        }
    }
}
