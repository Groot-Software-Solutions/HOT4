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
    public class BankTrxs : Table<BankTrx>, IBankTrxs
    {
        public BankTrxs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.GetSuffix = "_GetFromTrxId";
            base.AddSuffix = "_Insert";
            base.AddParameters = "@BankTrxBatchID,@BankTrxTypeID,@BankTrxStateID,@Amount,@TrxDate,@Identifier,@RefName,@Branch,@BankRef,@Balance";
            base.UpdateSuffix = "_Save";
            base.UpdateParameters = "@BankTrxID,@BankTrxBatchID,@BankTrxTypeID,@BankTrxStateID,@Amount,@TrxDate,@Identifier,@RefName,@Branch,@BankRef,@Balance,@PaymentID";

            
        }

        public override async Task<OneOf<BankTrx, HotDbException>> GetAsync(int Id)
        {
            string query = $"{StoredProcedurePrefix}{typeof(BankTrx).Name}{GetSuffix} @vPaymentID";
            var parameters = new Dictionary<string, object>() { { $"@vPaymentID", Id } };
            var result = await dbHelper.QuerySingle<BankTrx>(query, parameters);
            return result;
        }
        public async Task<OneOf<List<BankTrx>, HotDbException>> ListAsync(BankTrxBatch bankTrxBatch)
        {
            return await ListAsync(bankTrxBatch.BankTrxBatchID);
        }

        public async Task<OneOf<List<BankTrx>, HotDbException>> ListAsync(long bankTrxBatchId)
        {
            string Query = $"{GetSPPrefix()}_List @BankTrxBatchID";
            return await dbHelper.Query<BankTrx>(Query, new { bankTrxBatchId });
        }

        public async Task<OneOf<List<BankTrx>, HotDbException>> ListPendingAsync(long bankTrxBatchId)
        {
            string Query = $"{GetSPPrefix()}_List_Pending @BankTrxBatchID";
            return await dbHelper.Query<BankTrx>(Query, new { bankTrxBatchId });
        }

        public async Task<OneOf<List<BankTrx>, HotDbException>> ListPendingAsync(BankTrxBatch bankTrxBatch)
        {
            return await ListPendingAsync(bankTrxBatch.BankTrxBatchID);
        }

        public async Task<OneOf<List<BankTrx>, HotDbException>> ListPendingEcocashAsync()
        {
            string Query = $"{GetSPPrefix()}_ListPendingEcocash";
            return await dbHelper.Query<BankTrx>(Query);
        }
        public async Task<OneOf<List<BankTrx>, HotDbException>> ListPendingOneMoneyAsync()
        {
            string Query = $"{GetSPPrefix()}_ListPendingOneMoney";
            return await dbHelper.Query<BankTrx>(Query);
        }



        public OneOf<List<BankTrx>, HotDbException> ListPending(long bankTrxBatchId)
            => ListPendingAsync(bankTrxBatchId).Result;

        public OneOf<List<BankTrx>, HotDbException> ListPending(BankTrxBatch bankTrxBatch)
            => ListPendingAsync(bankTrxBatch).Result;

        public OneOf<List<BankTrx>, HotDbException> List(BankTrxBatch bankTrxBatch)
            => ListPendingAsync(bankTrxBatch).Result;

        public OneOf<List<BankTrx>, HotDbException> List(long bankTrxBatchId)
            => ListAsync(bankTrxBatchId).Result;

        public async Task<OneOf<BankTrx, HotDbException>> GetByRefAsync(string bankRef)
        {
            string Query = $"{GetSPPrefix()}_GetFromBankRef @bankRef";
            return await dbHelper.QuerySingle<BankTrx>(Query, new { bankRef });
        }

        public OneOf<List<BankTrx>, HotDbException> ListPendingEcocash() 
            => ListPendingEcocashAsync().Result;

        public OneOf<List<BankTrx>, HotDbException> ListPendingOneMoney() 
            => ListPendingOneMoneyAsync().Result;

        public async Task<OneOf<List<BankTrx>, HotDbException>> ListNewOneMoneyAsync()
        {
            string Query = $"{GetSPPrefix()}_ListNewOneMoney";
            return await dbHelper.Query<BankTrx>(Query);
        }

        public async Task<OneOf<List<BankTrx>, HotDbException>> ListNewEcocashAsync()
        {
            string Query = $"{GetSPPrefix()}_ListNewEcocash";
            return await dbHelper.Query<BankTrx>(Query);
        }
    }
}
