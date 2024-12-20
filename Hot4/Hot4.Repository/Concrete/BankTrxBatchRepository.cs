using Hot4.Core.DataViewModels;
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
        public async Task<BankTrxBatch> AddBatch(BankTrxBatch bankTrxBatch)
        {
            await Create(bankTrxBatch);
            await SaveChanges();
            return bankTrxBatch;
        }
        public async Task<List<BankBatchModel>> GetBatchByBank(byte bankId)
        {
            return await _context.BankTrxBatch.Include(d => d.Bank).Where(d => d.BankId == bankId)
            .OrderBy(d => d.BatchDate).Select(d => new BankBatchModel
            {
                BankTrxBatchID = d.BankTrxBatchId,
                BankID = d.BankId,
                BankName = d.Bank.Bank,
                BatchDate = d.BatchDate,
                BatchReference = d.BatchReference,
                LastUser = d.LastUser
            }).ToListAsync();
        }
        public async Task<long?> GetCurrentBatchIdByBank(byte bankId)
        {
            var dateNow = DateTime.Now.Date;
            var startOfDay = dateNow;
            var endOfDay = dateNow.AddDays(1).AddTicks(-1);

            var bankTrxBatchId = await GetByCondition(d => d.BankId == bankId && d.BatchDate >= startOfDay && d.BatchDate <= endOfDay)
                .OrderByDescending(d => d.BankTrxBatchId).Select(d => d.BankTrxBatchId).FirstOrDefaultAsync();
            return bankTrxBatchId;
        }
        public async Task<long?> GetCurrentBatchIdByBankAndRef(byte bankId, string batchReference)
        {
            var dateNow = DateTime.Now.Date;
            var startOfDay = dateNow;
            var endOfDay = dateNow.AddDays(1).AddTicks(-1);

            var bankTrxBatchId = await GetByCondition(d => d.BankId == bankId && d.BatchReference == batchReference &&
            d.BatchDate >= startOfDay && d.BatchDate <= endOfDay)
                .OrderByDescending(d => d.BankTrxBatchId).Select(d => d.BankTrxBatchId).FirstOrDefaultAsync();
            return bankTrxBatchId;
        }
        public async Task<BankTrxBatch?> GetCurrentBatch(byte bankId, string batchReference, string lastUser)
        {
            long? bankTrxBatchId = null;
            if (bankId == (int)BankName.EcoMerchant)
            {
                bankTrxBatchId = await GetCurrentBatchIdByBank(bankId);
            }
            else
            {
                bankTrxBatchId = await GetCurrentBatchIdByBankAndRef(bankId, batchReference);
            }
            if (bankTrxBatchId == null || bankTrxBatchId == 0)
            {
                var model = new BankTrxBatch() { BankId = bankId, BatchReference = batchReference, LastUser = lastUser, BatchDate = DateTime.Now };

                var res = await AddBatch(model);
                bankTrxBatchId = res.BankTrxBatchId;
            }
            return await GetById(bankTrxBatchId);
        }
    }
}
