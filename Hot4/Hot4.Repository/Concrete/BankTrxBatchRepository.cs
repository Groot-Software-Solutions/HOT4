using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankTrxBatchRepository : RepositoryBase<BankTrxBatch>, IBankTrxBatchRepository
    {
        public BankTrxBatchRepository(HotDbContext context) : base(context) { }
        public async Task<BankTrxBatch> AddBatch(BankTrxBatch bankTrxBatch)
        {
            await Create(bankTrxBatch);
            await SaveChanges();
            return bankTrxBatch;
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
        public async Task<List<BankBatchModel>> GetBatch_by_Bank(byte bankId)
        {
            return await _context.BankTrxBatch.Include(d => d.Bank).Where(d => d.BankId == bankId)
            .OrderBy(d => d.BatchDate).Select(d => new BankBatchModel
            {
                BankTrxBatchId = d.BankTrxBatchId,
                BankId = d.BankId,
                BankName = d.Bank.Bank,
                BatchDate = d.BatchDate,
                BatchReference = d.BatchReference,
                LastUser = d.LastUser
            }).ToListAsync();
        }
        public async Task<long?> GetCurrentBatchId_by_Bank_Ref(byte bankId, string batchRef = null)
        {
            var dateNow = DateTime.Now.Date;
            var startOfDay = dateNow;
            var endOfDay = dateNow.AddDays(1).AddTicks(-1);

            if (string.IsNullOrEmpty(batchRef))
            {
                var bankTrxBatchId = await GetByCondition(d => d.BankId == bankId && d.BatchDate >= startOfDay && d.BatchDate <= endOfDay)
                    .OrderByDescending(d => d.BankTrxBatchId).Select(d => d.BankTrxBatchId).FirstOrDefaultAsync();
            }
            else
            {
                var bankTrxBatchId = await GetByCondition(d => d.BankId == bankId && d.BatchReference == batchRef &&
            d.BatchDate >= startOfDay && d.BatchDate <= endOfDay)
                .OrderByDescending(d => d.BankTrxBatchId).Select(d => d.BankTrxBatchId).FirstOrDefaultAsync();
                return bankTrxBatchId;
            }
            return null;
        }

        public async Task<BankTrxBatch?> GetCurrentBatch(byte bankId, string batchReference, string lastUser)
        {
            long? bankTrxBatchId = null;
            if (bankId == (int)BankName.EcoMerchant)
            {
                bankTrxBatchId = await GetCurrentBatchId_by_Bank_Ref(bankId);
            }
            else
            {
                bankTrxBatchId = await GetCurrentBatchId_by_Bank_Ref(bankId, batchReference);
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

                var res = await AddBatch(model);
                bankTrxBatchId = res.BankTrxBatchId;
            }
            return await GetById(bankTrxBatchId);
        }
    }
}
