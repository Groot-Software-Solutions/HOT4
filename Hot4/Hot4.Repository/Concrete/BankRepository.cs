using Hot4.Core.DataViewModels;
using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankRepository : RepositoryBase<TblBankTrx>, IBankRepository
    {
        public BankRepository(HotDbContext context) : base(context) { }
        public async Task AddBankTrx(TblBankTrx bankTrx)
        {
            var duplicateTransaction = await GetByCondition(d =>
                     (
                     d.TrxDate == bankTrx.TrxDate
                     && d.Amount == bankTrx.Amount
                     && d.Identifier == bankTrx.Identifier
                     && d.BankRef == bankTrx.BankRef
                     && d.RefName == bankTrx.RefName
                     && d.Balance == bankTrx.Balance
                     )
                 || (
                     d.BankTrxTypeId == (byte)BankTrxTypes.EcoCashPending
                     && d.Amount == bankTrx.Amount
                     && d.Identifier == bankTrx.Identifier
                     && d.BankRef == bankTrx.BankRef
                     && bankTrx.BankRef != "pending"
                     )
                 || (
                     d.BankTrxTypeId == (byte)BankTrxTypes.SalaryCredit
                     && d.Amount == bankTrx.Amount
                     && d.Identifier == bankTrx.Identifier
                     && d.BankRef == bankTrx.BankRef
                     )
                 || (
                     d.TrxDate == bankTrx.TrxDate
                     && d.Amount == bankTrx.Amount
                     && d.BankRef == bankTrx.BankRef
                     && d.Branch == bankTrx.Branch
                     && d.Balance == bankTrx.Balance
                     )).FirstOrDefaultAsync();

            if (duplicateTransaction == null)
            {
                await Create(bankTrx);
                await SaveChanges();
            }
        }

        public async Task<TblBankTrxBatch> AddBankTrxBatch(TblBankTrxBatch tblBankTrxBatch)
        {
            await _context.BankTrxBatch.AddAsync(tblBankTrxBatch);
            await _context.SaveChangesAsync();
            return tblBankTrxBatch;
        }
        public async Task UpdateBankTrx(TblBankTrx bankTrx)
        {
            await Update(bankTrx);
            await SaveChanges();
        }

        public async Task UpdateIdentifierDBankTrx(string identifier, long bankTrxId)
        {
            var existing = await GetById(bankTrxId);
            if (existing != null)
            {
                existing.Identifier = identifier;
                await Update(existing);
                await SaveChanges();
            }

        }

        public async Task UpdatePaymentIDBankTrx(long paymentId, long bankTrxId)
        {
            var existing = await GetById(bankTrxId);
            if (existing != null)
            {
                existing.PaymentId = paymentId;
                await Update(existing);
                await SaveChanges();
            }
        }

        public async Task UpdateStateDBankTrx(byte bankTrxStateId, long bankTrxId)
        {
            var existing = await GetById(bankTrxId);
            if (existing != null)
            {
                existing.BankTrxStateId = bankTrxStateId;
                await Update(existing);
                await SaveChanges();
            }
        }
        public async Task<TblBankTrxBatch?> GetBatch(byte bankId, string BatchReference)
        {
            return await _context.BankTrxBatch
                  .SingleOrDefaultAsync(d =>
                     d.BankId == bankId
                  && d.BatchReference == BatchReference);
        }

        public async Task<List<TblBank>> ListBanks()
        {
            return await _context.Bank.OrderBy(d => d.Bank).ToListAsync();
        }

        public async Task<List<BankBatchModel>> ListBankTransactionBatches(long bankId)
        {
            return await (from batch in _context.BankTrxBatch
                          join bank in _context.Bank on batch.BankId equals bank.BankId
                          where batch.BankId == bankId
                          orderby batch.BatchDate
                          select new BankBatchModel
                          {
                              BankTrxBatchID = batch.BankTrxBatchId,
                              BankID = bank.BankId,
                              BatchDate = batch.BatchDate,
                              BatchReference = batch.BatchReference,
                              LastUser = batch.LastUser
                          }).ToListAsync();
        }

        public async Task<List<BankTransactionModel>> ListBankTransactions(long bankTrxBatchId)
        {
            return await (from trx in _context.BankTrx
                          join state in _context.BankTrxState on trx.BankTrxStateId equals state.BankTrxStateId
                          join type in _context.BankTrxType on trx.BankTrxTypeId equals type.BankTrxTypeId
                          where trx.BankTrxBatchId == bankTrxBatchId
                          select new BankTransactionModel
                          {
                              BankTrxID = trx.BankTrxId,
                              BankTrxBatchID = trx.BankTrxBatchId,
                              BankTrxTypeID = type.BankTrxTypeId,
                              BankTrxType = type.BankTrxType,
                              BankTrxStateID = state.BankTrxStateId,
                              BankTrxState = state.BankTrxState,
                              Amount = trx.Amount,
                              TrxDate = trx.TrxDate,
                              Identifier = trx.Identifier,
                              RefName = trx.RefName,
                              Branch = trx.Branch,
                              BankRef = trx.BankRef,
                              Balance = trx.Balance,
                              PaymentID = trx.PaymentId
                          }
                       ).ToListAsync();
        }


    }
}
