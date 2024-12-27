using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxBatchRepository : RepositoryBase<BankTrxBatch>, IBankTrxBatchRepository
    {
        public BankTrxBatchRepository(HotDbContext context) : base(context) { }
        public async Task<long> AddBatch(BankTrxBatch bankTrxBatch)
        {
            await Create(bankTrxBatch);
            await SaveChanges();

            return bankTrxBatch.BankTrxBatchId;
        }
        public async Task UpdateBatch(BankTrxBatch bankTrxBatch)
        {
            var bankBatch = await GetById(bankTrxBatch.BankTrxBatchId);
            if (bankBatch != null)
            {
                await Update(bankTrxBatch);
                await SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Batch record not found.");
            }
        }
        public async Task DeleteBatch(BankTrxBatch bankTrxBatch)
        {
            var bankBatch = await GetById(bankTrxBatch.BankTrxBatchId);
            if (bankBatch != null)
            {
                await Delete(bankTrxBatch);
                await SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Batch record not found.");
            }
        }
        public async Task<List<BankBatchModel>> GetBatchByBank(byte bankId)
        {
            return await GetByCondition(d => d.BankId == bankId)
                .Include(d => d.Bank)
                .OrderBy(d => d.BatchDate)
                               .Select(d => new BankBatchModel
                               {
                                   BankTrxBatchId = d.BankTrxBatchId,
                                   BankId = d.BankId,
                                   BankName = d.Bank.Bank,
                                   BatchDate = d.BatchDate,
                                   BatchReference = d.BatchReference,
                                   LastUser = d.LastUser
                               }).ToListAsync();
        }
        public async Task<long?> GetCurrentBatchIdByBankRef(byte bankId, string batchRef = null)
        {
            var dateNow = DateTime.Now.Date;
            var startOfDay = dateNow;
            var endOfDay = dateNow.AddDays(1).AddTicks(-1);

            if (string.IsNullOrEmpty(batchRef))
            {
                var bankTrxBatchId = await _context.BankTrxBatch.LastOrDefaultAsync(d => d.BankId == bankId
                && d.BatchDate >= startOfDay
                && d.BatchDate <= endOfDay);

                return bankTrxBatchId?.BankTrxBatchId;
            }
            else
            {
                var bankTrxBatchId = await _context.BankTrxBatch.LastOrDefaultAsync(d => d.BankId == bankId && d.BatchReference == batchRef
                && d.BatchDate >= startOfDay && d.BatchDate <= endOfDay);

                return bankTrxBatchId?.BankTrxBatchId;
            }
        }

        public async Task<BankBatchModel?> GetCurrentBatch(byte bankId, string batchReference, string lastUser)
        {
            long? bankTrxBatchId = null;
            if (bankId == (int)BankName.EcoMerchant)
            {
                bankTrxBatchId = await GetCurrentBatchIdByBankRef(bankId);
            }
            else
            {
                bankTrxBatchId = await GetCurrentBatchIdByBankRef(bankId, batchReference);
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

                bankTrxBatchId = await AddBatch(model);

            }
            var result = await GetById(bankTrxBatchId);
            if (result != null)
            {
                return new BankBatchModel
                {
                    BankId = result.BankId,
                    BankTrxBatchId = result.BankTrxBatchId,
                    BatchDate = result.BatchDate,
                    BatchReference = result.BatchReference,
                    LastUser = result.LastUser
                };
            }
            else
            {
                return null;
            }
        }
    }
}
