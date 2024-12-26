using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel.ApiModels;
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
        public async Task<BankTransactionModel> GetTranscation_by_Id(long bankTransactionId)
        {
            var result = await _context.BankTrx.Include(d => d.BankTrxState).Include(d => d.BankTrxType)
               .FirstOrDefaultAsync(d => d.BankTrxId == bankTransactionId);
            if (result != null)
            {
                return new BankTransactionModel
                {
                    Amount = result.Amount,
                    Balance = result.Balance,
                    BankRef = result.BankRef,
                    BankTrxBatchId = result.BankTrxBatchId,
                    BankTrxId = result.BankTrxId,
                    BankTrxState = result.BankTrxState.BankTrxState,
                    BankTrxStateId = result.BankTrxStateId,
                    BankTrxType = result.BankTrxType.BankTrxType,
                    BankTrxTypeId = result.BankTrxTypeId,
                    Branch = result.Branch,
                    Identifier = result.Identifier,
                    PaymentId = result.PaymentId,
                    RefName = result.RefName,
                    TrxDate = result.TrxDate,

                };
            }
            else
            {
                return new BankTransactionModel();
            }
        }
        public async Task<List<BankTransactionModel>> GetTranscation_by_Batch(long bankTransactionBatchId, bool isPending)
        {
            if (isPending)
            {
                return await GetByCondition(d => d.BankTrxBatchId == bankTransactionBatchId
                && !new[] { (int)BankTransactionStates.Pending, (int)BankTransactionStates.BusyConfirming }.Contains(d.BankTrxStateId))
                    .Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                                                   .Select(d => new BankTransactionModel
                                                   {
                                                       Amount = d.Amount,
                                                       Balance = d.Balance,
                                                       BankRef = d.BankRef,
                                                       BankTrxBatchId = d.BankTrxBatchId,
                                                       BankTrxId = d.BankTrxId,
                                                       BankTrxState = d.BankTrxState.BankTrxState,
                                                       BankTrxStateId = d.BankTrxStateId,
                                                       BankTrxType = d.BankTrxType.BankTrxType,
                                                       BankTrxTypeId = d.BankTrxTypeId,
                                                       Branch = d.Branch,
                                                       Identifier = d.Identifier,
                                                       PaymentId = d.PaymentId,
                                                       RefName = d.RefName,
                                                       TrxDate = d.TrxDate
                                                   }).OrderByDescending(d => d.BankTrxId).ToListAsync();

            }
            else
            {
                return await GetByCondition(d => d.BankTrxBatchId == bankTransactionBatchId)
                    .Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                                         .Select(d => new BankTransactionModel
                                         {
                                             Amount = d.Amount,
                                             Balance = d.Balance,
                                             BankRef = d.BankRef,
                                             BankTrxBatchId = d.BankTrxBatchId,
                                             BankTrxId = d.BankTrxId,
                                             BankTrxState = d.BankTrxState.BankTrxState,
                                             BankTrxStateId = d.BankTrxStateId,
                                             BankTrxType = d.BankTrxType.BankTrxType,
                                             BankTrxTypeId = d.BankTrxTypeId,
                                             Branch = d.Branch,
                                             Identifier = d.Identifier,
                                             PaymentId = d.PaymentId,
                                             RefName = d.RefName,
                                             TrxDate = d.TrxDate
                                         }).OrderByDescending(d => d.BankTrxId).ToListAsync();
            }
        }

        public async Task<List<BankTransactionModel>> GetPendingTranscation_by_Type(byte bankTransactionTypeId)
        {
            if (bankTransactionTypeId == (int)BankTransactionTypes.EcoCashPending)
            {
                return await GetByCondition(d => d.BankTrxTypeId == bankTransactionTypeId
                    && d.BankTrxStateId == (int)BankTransactionStates.BusyConfirming
                    && d.TrxDate > DateTime.Now.AddDays(-7))
                    .Include(d => d.BankTrxState).Include(d => d.BankTrxType)

                                                .Select(d => new BankTransactionModel
                                                {
                                                    Amount = d.Amount,
                                                    Balance = d.Balance,
                                                    BankRef = d.BankRef,
                                                    BankTrxBatchId = d.BankTrxBatchId,
                                                    BankTrxId = d.BankTrxId,
                                                    BankTrxState = d.BankTrxState.BankTrxState,
                                                    BankTrxStateId = d.BankTrxStateId,
                                                    BankTrxType = d.BankTrxType.BankTrxType,
                                                    BankTrxTypeId = d.BankTrxTypeId,
                                                    Branch = d.Branch,
                                                    Identifier = d.Identifier,
                                                    PaymentId = d.PaymentId,
                                                    RefName = d.RefName,
                                                    TrxDate = d.TrxDate
                                                }).OrderByDescending(d => d.BankTrxId).ToListAsync();
            }
            else
            {
                return await GetByCondition(d => d.BankTrxTypeId == bankTransactionTypeId
                    && d.BankTrxStateId == (int)BankTransactionStates.BusyConfirming)
                   .Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                                       .Select(d => new BankTransactionModel
                                       {
                                           Amount = d.Amount,
                                           Balance = d.Balance,
                                           BankRef = d.BankRef,
                                           BankTrxBatchId = d.BankTrxBatchId,
                                           BankTrxId = d.BankTrxId,
                                           BankTrxState = d.BankTrxState.BankTrxState,
                                           BankTrxStateId = d.BankTrxStateId,
                                           BankTrxType = d.BankTrxType.BankTrxType,
                                           BankTrxTypeId = d.BankTrxTypeId,
                                           Branch = d.Branch,
                                           Identifier = d.Identifier,
                                           PaymentId = d.PaymentId,
                                           RefName = d.RefName,
                                           TrxDate = d.TrxDate
                                       }).OrderByDescending(d => d.BankTrxId).ToListAsync();
            }
        }
        public async Task<List<BankTransactionModel>> GetAllTranscation_by_Type(byte bankTransactionTypeId) // pending state 0
        {
            return await GetByCondition(d => d.BankTrxTypeId == bankTransactionTypeId
                                && d.BankTrxStateId == (int)BankTransactionStates.Pending)
                .Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                                                .Select(d => new BankTransactionModel
                                                {
                                                    Amount = d.Amount,
                                                    Balance = d.Balance,
                                                    BankRef = d.BankRef,
                                                    BankTrxBatchId = d.BankTrxBatchId,
                                                    BankTrxId = d.BankTrxId,
                                                    BankTrxState = d.BankTrxState.BankTrxState,
                                                    BankTrxStateId = d.BankTrxStateId,
                                                    BankTrxType = d.BankTrxType.BankTrxType,
                                                    BankTrxTypeId = d.BankTrxTypeId,
                                                    Branch = d.Branch,
                                                    Identifier = d.Identifier,
                                                    PaymentId = d.PaymentId,
                                                    RefName = d.RefName,
                                                    TrxDate = d.TrxDate
                                                }).OrderByDescending(d => d.BankTrxId).ToListAsync();
        }
        public async Task<List<BankTransactionModel>> GetTranscation_by_Ref(string bankRef)
        {
            return await GetByCondition(d => d.BankRef == bankRef && d.BankTrxStateId == (int)BankTransactionStates.BusyConfirming
                               && d.BankTrxTypeId == (int)BankTransactionTypes.EcoCashPending)
               .Include(d => d.BankTrxState).Include(d => d.BankTrxType)
                                               .Select(d => new BankTransactionModel
                                               {
                                                   Amount = d.Amount,
                                                   Balance = d.Balance,
                                                   BankRef = d.BankRef,
                                                   BankTrxBatchId = d.BankTrxBatchId,
                                                   BankTrxId = d.BankTrxId,
                                                   BankTrxState = d.BankTrxState.BankTrxState,
                                                   BankTrxStateId = d.BankTrxStateId,
                                                   BankTrxType = d.BankTrxType.BankTrxType,
                                                   BankTrxTypeId = d.BankTrxTypeId,
                                                   Branch = d.Branch,
                                                   Identifier = d.Identifier,
                                                   PaymentId = d.PaymentId,
                                                   RefName = d.RefName,
                                                   TrxDate = d.TrxDate
                                               }).OrderByDescending(d => d.BankTrxId).ToListAsync();
        }

        public async Task<long?> GetDuplicateTranscation(BankTransactionSearchModel bankTransactionSearch)
        {
            var bankBatchDetail = await _batchRepository.GetBatch_by_Bank(bankTransactionSearch.BankId);
            var result = await _context.BankTrx.FirstOrDefaultAsync(d => d.Amount == bankTransactionSearch.Amount
            && d.TrxDate == bankTransactionSearch.TrxDate && d.Balance == bankTransactionSearch.Balance
            && d.BankRef == bankTransactionSearch.BankRef && bankBatchDetail.Select(m => m.BankTrxBatchId).Contains(d.BankTrxBatchId));

            if (result != null)
            {
                return result.BankTrxId;
            }
            return null;
        }
        public async Task<List<BankTransactionModel>> GetTranscation_by_PaymentId(string paymentId)
        {
            var vpaymentId = new Guid(paymentId);

            return await (from bnkTrx in _context.BankTrx
                          join bp in _context.BankvPayment on bnkTrx.BankTrxId equals bp.BankTrxId
                          where bp.VPaymentId == vpaymentId
                          select bnkTrx
                          )
                                           .Select(d => new BankTransactionModel
                                           {
                                               Amount = d.Amount,
                                               Balance = d.Balance,
                                               BankRef = d.BankRef,
                                               BankTrxBatchId = d.BankTrxBatchId,
                                               BankTrxId = d.BankTrxId,
                                               BankTrxState = d.BankTrxState.BankTrxState,
                                               BankTrxStateId = d.BankTrxStateId,
                                               BankTrxType = d.BankTrxType.BankTrxType,
                                               BankTrxTypeId = d.BankTrxTypeId,
                                               Branch = d.Branch,
                                               Identifier = d.Identifier,
                                               PaymentId = d.PaymentId,
                                               RefName = d.RefName,
                                               TrxDate = d.TrxDate
                                           }).OrderByDescending(d => d.BankTrxId).ToListAsync();
        }
        public async Task<int?> GetEcoCashPendingTranscationCount(EcoCashSearchModel ecoCashSearch)
        {
            return await GetByCondition(d => d.BankTrxTypeId == (int)BankTransactionTypes.EcoCashPending
            && d.TrxDate > ecoCashSearch.date.AddHours(-1) && d.Identifier == ecoCashSearch.Mobile
            && d.Amount == ecoCashSearch.Amount).CountAsync();
        }
        public async Task<long?> AddBankTransaction(BankTrx bankTransaction)
        {
            var duplicateTransaction = await _context.BankTrx.FirstOrDefaultAsync(d =>
              (d.TrxDate == bankTransaction.TrxDate && d.Amount == bankTransaction.Amount && d.Identifier == bankTransaction.Identifier
              && d.BankRef == bankTransaction.BankRef && d.RefName == bankTransaction.RefName && d.Balance == bankTransaction.Balance)
          || (d.BankTrxTypeId == (byte)BankTransactionTypes.EcoCashPending && d.Amount == bankTransaction.Amount
          && d.Identifier == bankTransaction.Identifier && d.BankRef == bankTransaction.BankRef && bankTransaction.BankRef != "pending")
          || (d.BankTrxTypeId == (byte)BankTransactionTypes.SalaryCredit && d.Amount == bankTransaction.Amount
              && d.Identifier == bankTransaction.Identifier && d.BankRef == bankTransaction.BankRef)
          || (d.TrxDate == bankTransaction.TrxDate && d.Amount == bankTransaction.Amount
              && d.BankRef == bankTransaction.BankRef && d.Branch == bankTransaction.Branch && d.Balance == bankTransaction.Balance)
              );

            if (duplicateTransaction == null)
            {
                await Create(bankTransaction);
                await SaveChanges();
                return bankTransaction.BankTrxId;
            }
            else
            {
                return duplicateTransaction.BankTrxId;
            }
        }

        public async Task UpdateBankTransaction(BankTrx bankTransaction)
        {
            await Update(bankTransaction);
            await SaveChanges();
        }
        public async Task UpdateBankTransaction_PaymentId(long paymentId, long bankTransactionId)
        {
            var bankTransaction = await GetById(bankTransactionId);
            if (bankTransaction != null)
            {
                bankTransaction.PaymentId = paymentId;
                await Update(bankTransaction);
                await SaveChanges();
            }
        }
        public async Task UpdateBankTransaction_State(byte stateId, long bankTransactionId)
        {
            var bankTransaction = await GetById(bankTransactionId);
            if (bankTransaction != null)
            {
                bankTransaction.BankTrxStateId = stateId;
                await Update(bankTransaction);
                await SaveChanges();
            }
        }
        public async Task UpdateBankTransaction_Identifier(string identifier, long bankTransactionId)
        {
            var bankTransaction = await GetById(bankTransactionId);
            if (bankTransaction != null)
            {
                bankTransaction.Identifier = identifier;
                await Update(bankTransaction);
                await SaveChanges();
            }
        }
    }
}
