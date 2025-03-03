namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface  IBankTransactionType: IDbContextTable<BankTransactionType>
        , IDbCanList<BankTransactionType>
    {
    }
}
