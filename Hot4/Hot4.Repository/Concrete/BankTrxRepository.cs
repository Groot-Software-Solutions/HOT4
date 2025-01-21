using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;
using Microsoft.EntityFrameworkCore;
namespace Hot4.Repository.Concrete
{
    public class BankTrxRepository : RepositoryBase<BankTrx>, IBankTrxRepository
    {
        private IBankTrxBatchRepository _batchRepository;
        public BankTrxRepository(HotDbContext context, IBankTrxBatchRepository batchRepository) : base(context)
        {
            _batchRepository = batchRepository;
        }
        public async Task<BankTrx?> GetTrxById(long bankTransactionId)
        {
            return await _context.BankTrx
                         .Include(d => d.BankTrxState)
                         .Include(d => d.BankTrxType)
                         .FirstOrDefaultAsync(d => d.BankTrxId == bankTransactionId);
        }
        public async Task<List<BankTrx>> GetTrxByBatchId(long bankTransactionBatchId, bool isPending)
        {
            if (isPending)
            {
                return await GetByCondition(d => d.BankTrxBatchId == bankTransactionBatchId
                              && !new[]
                              {
                                  (int)BankTransactionStates.Pending, (int)BankTransactionStates.BusyConfirming
                              }.Contains(d.BankTrxStateId))
                               .Include(d => d.BankTrxState)
                               .Include(d => d.BankTrxType)
                               .OrderByDescending(d => d.BankTrxId)
                               .ToListAsync();
            }
            else
            {
                return await GetByCondition(d => d.BankTrxBatchId == bankTransactionBatchId)
                             .Include(d => d.BankTrxState)
                             .Include(d => d.BankTrxType)
                             .OrderByDescending(d => d.BankTrxId)
                             .ToListAsync();
            }
        }
        public async Task<List<BankTrx>> GetPendingTrxByType(byte bankTransactionTypeId)
        {
            if (bankTransactionTypeId == (int)BankTransactionTypes.EcoCash)
            {
                return await GetByCondition(d => d.BankTrxTypeId == bankTransactionTypeId
                             && d.BankTrxStateId == (int)BankTransactionStates.BusyConfirming
                             && d.TrxDate > DateTime.Now.AddDays(-7))
                             .Include(d => d.BankTrxState)
                             .Include(d => d.BankTrxType)
                             .OrderByDescending(d => d.BankTrxId)
                             .ToListAsync();
            }
            else
            {
                return await GetByCondition(d => d.BankTrxTypeId == bankTransactionTypeId
                             && d.BankTrxStateId == (int)BankTransactionStates.BusyConfirming)
                             .Include(d => d.BankTrxState)
                             .Include(d => d.BankTrxType)
                             .OrderByDescending(d => d.BankTrxId)
                             .ToListAsync();
            }
        }
        public async Task<List<BankTrx>> GetAllTrxByType(byte bankTransactionTypeId) // pending state 0
        {
            return await GetByCondition(d => d.BankTrxTypeId == bankTransactionTypeId
                         && d.BankTrxStateId == (int)BankTransactionStates.Pending)
                         .Include(d => d.BankTrxState)
                         .Include(d => d.BankTrxType)
                         .OrderByDescending(d => d.BankTrxId)
                         .ToListAsync();

        }
        public async Task<BankTrx?> GetTrxByRef(string bankRef)
        {
            return await _context.BankTrx.Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                          .LastOrDefaultAsync(d => d.BankRef == bankRef
                         && d.BankTrxStateId == (int)BankTransactionStates.BusyConfirming
                         && d.BankTrxTypeId == (int)BankTransactionTypes.EcoCash);

        }

        public async Task<long?> GetDuplicateTrx(BankTransactionSearchModel bankTransactionSearch)
        {
            var bankBatchDetail = await _batchRepository.GetBatchByBankId(bankTransactionSearch.BankId);
            if (bankBatchDetail != null && bankBatchDetail.Any())
            {
                var result = await _context.BankTrx.FirstOrDefaultAsync(d => d.Amount == bankTransactionSearch.Amount
                                    && d.TrxDate == bankTransactionSearch.TrxDate
                                    && d.Balance == bankTransactionSearch.Balance
                                    && d.BankRef == bankTransactionSearch.BankRef
                                    && EF.Constant(bankBatchDetail.Select(m => m.BankTrxBatchId)).Contains(d.BankTrxBatchId));

                if (result != null)
                {
                    return result.BankTrxId;
                }
            }
            return null;
        }
        public async Task<List<BankTrx>> GetTrxByPaymentId(string paymentId)
        {
            var vpaymentId = new Guid(paymentId);

            return await (from bnkTrx in _context.BankTrx.Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                          join bp in _context.BankvPayment on bnkTrx.BankTrxId equals bp.BankTrxId
                          where bp.VPaymentId == vpaymentId
                          orderby bnkTrx.BankTrxId descending
                          select bnkTrx).ToListAsync();

        }
        public async Task<int?> GetEcoCashPendingTrxCount(EcoCashSearchModel ecoCashSearch)
        {
            return await GetByCondition(d => d.BankTrxTypeId == (int)BankTransactionTypes.EcoCash
                         && d.TrxDate > ecoCashSearch.date.AddHours(-1)
                         && d.Identifier == ecoCashSearch.Mobile
                         && d.Amount == ecoCashSearch.Amount)
                         .CountAsync();
        }
        public async Task<bool> AddBankTrx(BankTrx bankTransaction)
        {
            var duplicateTransaction = await _context.BankTrx.Include(d => d.BankTrxBatch).FirstOrDefaultAsync(d =>
                                       (d.TrxDate == bankTransaction.TrxDate
                                        && d.Amount == bankTransaction.Amount
                                        && d.Identifier == bankTransaction.Identifier
                                        && d.BankRef == bankTransaction.BankRef
                                        && d.RefName == bankTransaction.RefName
                                        && d.Balance == bankTransaction.Balance)
                                       ||
                                       (d.BankTrxTypeId == (byte)BankTransactionTypes.EcoCash
                                       && d.Amount == bankTransaction.Amount
                                       && d.Identifier == bankTransaction.Identifier
                                       && d.BankRef == bankTransaction.BankRef
                                       && bankTransaction.BankRef != "pending")
                                       ||
                                       (d.BankTrxBatch.BankId == (byte)BankName.CBZ
                                       && d.Amount == bankTransaction.Amount
                                       && d.Identifier == bankTransaction.Identifier
                                       && d.BankRef == bankTransaction.BankRef)
                                       ||
                                       (d.TrxDate == bankTransaction.TrxDate
                                       && d.Amount == bankTransaction.Amount
                                       && d.BankRef == bankTransaction.BankRef
                                       && d.Branch == bankTransaction.Branch
                                       && d.Balance == bankTransaction.Balance));

            if (duplicateTransaction == null)
            {
                bankTransaction.PaymentId = null;
                await Create(bankTransaction);
                await SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateBankTrx(BankTrx bankTransaction)
        {
            Update(bankTransaction);
            await SaveChanges();
            return true;
        }

    }
}
