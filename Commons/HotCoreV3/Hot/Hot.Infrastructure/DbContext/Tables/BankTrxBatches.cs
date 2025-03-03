using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class BankTrxBatches : Table<BankTrxBatch>, IBankTrxBatches
    {
        public BankTrxBatches(IDbHelper dbHelper) : base(dbHelper)
        { 
            base.StoredProcedurePrefix = "x";
          
        }
         
        public async Task<OneOf<BankTrxBatch, HotDbException>> GetCurrentBatchAsync(BankTrxBatch batch)
        {
            return await GetCurrentBatchAsync(batch.BankID, batch.BatchReference, batch.LastUser);
        }

        public async Task<OneOf<BankTrxBatch, HotDbException>> GetCurrentBatchAsync(int BankID, string BatchReference, string LastUser)
        {
            string Query = $"{GetSPPrefix()}_GetCurrentBatch @BankID,@BatchReference,@LastUser";
            return await dbHelper.QuerySingle<BankTrxBatch>(Query, new { BankID, BatchReference, LastUser });
        }

        public OneOf<BankTrxBatch, HotDbException> GetCurrentBatch(BankTrxBatch batch)
           => GetCurrentBatchAsync(batch).Result;

        public OneOf<BankTrxBatch, HotDbException> GetCurrentBatch(int BankID, string BatchReference, string LastUser)
            => GetCurrentBatchAsync(BankID, BatchReference, LastUser).Result;

        public async Task<OneOf<List<BankTrxBatch>, HotDbException>> ListAsync(byte BankId)
        {
            string Query = $"{GetSPPrefix()}_list @BankID";
            return await dbHelper.Query<BankTrxBatch>(Query, new { BankId });
        }

        public OneOf<List<BankTrxBatch>, HotDbException> List(byte BankId)
        {
            return ListAsync(BankId).Result;
        }
    }
}
