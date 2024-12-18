﻿using Hot4.Core.DataViewModels;
using Hot4.Core.Enums;
using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Hot4.Repository.Concrete
{
    public class BankRepository : RepositoryBase<BankTrx>, IBankRepository
    {
        public BankRepository(HotDbContext context) : base(context) { }
        public async Task AddBankTrx(BankTrx bankTrx)
        {
            var duplicateTransaction = await _context.BankTrx.FirstOrDefaultAsync(d =>
                     (
                     d.TrxDate == bankTrx.TrxDate
                     && d.Amount == bankTrx.Amount
                     && d.Identifier == bankTrx.Identifier
                     && d.BankRef == bankTrx.BankRef
                     && d.RefName == bankTrx.RefName
                     && d.Balance == bankTrx.Balance
                     )
                 || (
                     d.BankTrxTypeId == (byte)BankTranType.EcoCashPending
                     && d.Amount == bankTrx.Amount
                     && d.Identifier == bankTrx.Identifier
                     && d.BankRef == bankTrx.BankRef
                     && bankTrx.BankRef != "pending"
                     )
                 || (
                     d.BankTrxTypeId == (byte)BankTranType.SalaryCredit
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
                     ));

            if (duplicateTransaction == null)
            {
                await Create(bankTrx);
                await SaveChanges();
            }
        }

        public async Task<BankTrxBatch> AddBankTrxBatch(BankTrxBatch tblBankTrxBatch)
        {
            await _context.BankTrxBatch.AddAsync(tblBankTrxBatch);
            await _context.SaveChangesAsync();
            return tblBankTrxBatch;
        }
        public async Task UpdateBankTrx(BankTrx bankTrx)
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
        public async Task<BankTrxBatch?> GetBatch(byte bankId, string BatchReference)
        {
            return await _context.BankTrxBatch.FirstOrDefaultAsync(d => d.BankId == bankId && d.BatchReference == BatchReference);
        }

        public async Task<List<DataModel.Models.Banks>> ListBanks()
        {
            return await _context.Bank.OrderBy(d => d.Bank).ToListAsync();
        }

        public async Task<List<BankBatchModel>> ListBankTransactionBatches(long bankId)
        {
            //return await (from batch in _context.BankTrxBatch
            //              join bank in _context.Bank on batch.BankId equals bank.BankId
            //              where batch.BankId == bankId
            //              orderby batch.BatchDate
            //              select new BankBatchModel
            //              {
            //                  BankTrxBatchID = batch.BankTrxBatchId,
            //                  BankID = bank.BankId,
            //                  BatchDate = batch.BatchDate,
            //                  BatchReference = batch.BatchReference,
            //                  LastUser = batch.LastUser
            //              }).ToListAsync();

            return await _context.BankTrxBatch.Include(d => d.Bank).Where(d => d.BankId == bankId)
                .OrderBy(d => d.BatchDate).Select(d => new BankBatchModel
                {
                    BankTrxBatchID = d.BankTrxBatchId,
                    BankID = d.BankId,
                    BatchDate = d.BatchDate,
                    BatchReference = d.BatchReference,
                    LastUser = d.LastUser
                }).ToListAsync();

        }

        public async Task<List<BankTransactionModel>> ListBankTransactions(long bankTrxBatchId)
        {
            return await _context.BankTrx.Include(d => new { d.BankTrxState, d.BankTrxType }).Where(d => d.BankTrxBatchId == bankTrxBatchId).Select(d => new BankTransactionModel
            {
                BankTrxID = d.BankTrxId,
                BankTrxBatchID = d.BankTrxBatchId,
                BankTrxTypeID = d.BankTrxType.BankTrxTypeId,
                BankTrxType = d.BankTrxType.BankTrxType,
                BankTrxStateID = d.BankTrxState.BankTrxStateId,
                BankTrxState = d.BankTrxState.BankTrxState,
                Amount = d.Amount,
                TrxDate = d.TrxDate,
                Identifier = d.Identifier,
                RefName = d.RefName,
                Branch = d.Branch,
                BankRef = d.BankRef,
                Balance = d.Balance,
                PaymentID = d.PaymentId
            }).ToListAsync();
            //return await (from trx in _context.BankTrx
            //              join state in _context.BankTrxState on trx.BankTrxStateId equals state.BankTrxStateId
            //              join type in _context.BankTrxType on trx.BankTrxTypeId equals type.BankTrxTypeId
            //              where trx.BankTrxBatchId == bankTrxBatchId
            //              select new BankTransactionModel
            //              {
            //                  BankTrxID = trx.BankTrxId,
            //                  BankTrxBatchID = trx.BankTrxBatchId,
            //                  BankTrxTypeID = type.BankTrxTypeId,
            //                  BankTrxType = type.BankTrxType,
            //                  BankTrxStateID = state.BankTrxStateId,
            //                  BankTrxState = state.BankTrxState,
            //                  Amount = trx.Amount,
            //                  TrxDate = trx.TrxDate,
            //                  Identifier = trx.Identifier,
            //                  RefName = trx.RefName,
            //                  Branch = trx.Branch,
            //                  BankRef = trx.BankRef,
            //                  Balance = trx.Balance,
            //                  PaymentID = trx.PaymentId
            //              }
            //           ).ToListAsync();
        }


    }
}
