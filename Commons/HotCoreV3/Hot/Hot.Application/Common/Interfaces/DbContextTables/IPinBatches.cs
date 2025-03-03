namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IPinBatches : IDbContextTable<PinBatch>
        , IDbCanAdd<PinBatch>
        , IDbCanAddInTransaction<PinBatch>
    {
        public Task<OneOf<List<PinBatch>, HotDbException>> ListAsync(int PinBatchTypeId);
        public OneOf<List<PinBatch>, HotDbException> List(int PinBatchTypeId);
    }

}
