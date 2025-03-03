namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IBankTrxs : IDbContextTable<BankTrx>
        , IDbCanAdd<BankTrx>
        , IDbCanAddInTransaction<BankTrx>
        , IDbCanGetById<BankTrx>
        , IDbCanRemoveById<BankTrx>
        , IDbCanUpdate<BankTrx> 
        , IDbCanUpdateInTransaction<BankTrx>
    {
        public Task<OneOf<List<BankTrx>, HotDbException>> ListAsync(BankTrxBatch bankTrxBatch);
        public Task<OneOf<List<BankTrx>, HotDbException>> ListAsync(long BankTrxBatchId);
        public Task<OneOf<List<BankTrx>, HotDbException>> ListPendingAsync(long BankTrxBatchId);
        public Task<OneOf<List<BankTrx>, HotDbException>> ListPendingAsync(BankTrxBatch bankTrxBatch);
        public Task<OneOf<List<BankTrx>, HotDbException>> ListPendingEcocashAsync(); 
        public Task<OneOf<List<BankTrx>, HotDbException>> ListPendingOneMoneyAsync();
        public Task<OneOf<List<BankTrx>, HotDbException>> ListNewOneMoneyAsync();
        public Task<OneOf<List<BankTrx>, HotDbException>> ListNewEcocashAsync();


        public Task<OneOf<BankTrx, HotDbException>> GetByRefAsync(string bankRef);
        public OneOf<List<BankTrx>, HotDbException> List(BankTrxBatch bankTrxBatch);
        public OneOf<List<BankTrx>, HotDbException> List(long BankTrxBatchId);
        public OneOf<List<BankTrx>, HotDbException> ListPending(long BankTrxBatchId);
        public OneOf<List<BankTrx>, HotDbException> ListPending(BankTrxBatch bankTrxBatch); 
        public OneOf<List<BankTrx>, HotDbException> ListPendingEcocash();
        public OneOf<List<BankTrx>, HotDbException> ListPendingOneMoney();
    }
}
