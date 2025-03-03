namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IBankTrxBatches : IDbContextTable<BankTrxBatch>
        , IDbCanAdd<BankTrxBatch>
        , IDbCanAddInTransaction<BankTrxBatch>
        , IDbCanGetById<BankTrxBatch>
        , IDbCanList<BankTrxBatch>
        , IDbCanRemoveById<BankTrxBatch>
        , IDbCanSearch<BankTrxBatch>
        , IDbCanUpdate<BankTrxBatch>
        , IDbCanUpdateInTransaction<BankTrxBatch>
    {
        public Task<OneOf<BankTrxBatch, HotDbException>> GetCurrentBatchAsync(BankTrxBatch batch);
        public Task<OneOf<BankTrxBatch, HotDbException>> GetCurrentBatchAsync(int BankID, string BatchReference, string LastUser);
        public Task<OneOf<List<BankTrxBatch>, HotDbException>> ListAsync(byte BankId);
        public OneOf<List<BankTrxBatch>, HotDbException> List(byte BankId);
        public OneOf<BankTrxBatch, HotDbException> GetCurrentBatch(BankTrxBatch batch);
        public OneOf<BankTrxBatch, HotDbException> GetCurrentBatch(int BankID, string BatchReference, string LastUser);
        
    }
}
