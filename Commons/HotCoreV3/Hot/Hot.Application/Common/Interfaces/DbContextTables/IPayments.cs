namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IPayments : IDbContextTable<Payment>
        , IDbCanAdd<Payment>
        , IDbCanAddInTransaction<Payment>
        , IDbCanGetById<Payment>
        , IDbCanList<Payment>
        , IDbCanRemoveById<Payment>
        , IDbCanSearch<Payment>
        , IDbCanUpdate<Payment>
        , IDbCanUpdateInTransaction<Payment>
    {
        public Task<OneOf<List<Payment>, HotDbException>> ListAsync(int AccountId);
        Task<OneOf<List<Payment>, HotDbException>> ListRecentAsync(int AccountId);
        public Task<OneOf<List<Payment>, HotDbException>> SearchOldAsync(string Filter);
    }
}
