namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IBankTransactionStates : IDbContextTable<BankTransactionState>
        , IDbCanList<BankTransactionState>
    {
    }
}
