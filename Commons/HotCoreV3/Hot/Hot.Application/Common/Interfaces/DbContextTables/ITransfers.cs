namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ITransfers : IDbContextTable<Transfer>
        , IDbCanAdd<Transfer>
        , IDbCanAddInTransaction<Transfer>
        , IDbCanGetById<Transfer>
        , IDbCanList<Transfer>
        , IDbCanRemoveById<Transfer>
        , IDbCanSearch<Transfer>
        , IDbCanUpdate<Transfer>
        , IDbCanUpdateInTransaction<Transfer>
    {
        public Task<OneOf<decimal, HotDbException>> StockTradeInBalanceAsync(long accountId);
        public OneOf<decimal, HotDbException> StockTradeInBalance(long accountId);

        public Task<OneOf<StockTradeInResult, HotDbException>> StockTradeInAsync(long accountId, decimal amount, decimal rate);
        public OneOf<StockTradeInResult, HotDbException> StockTradeIn(long accountId, decimal amount, decimal rate);

    }
}
