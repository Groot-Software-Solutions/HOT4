namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IPaymentTypes : IDbContextTable<PaymentType>
        , IDbCanAdd<PaymentType>
        , IDbCanAddInTransaction<PaymentType>
        , IDbCanGetById<PaymentType>
        , IDbCanList<PaymentType>
        , IDbCanRemoveById<PaymentType>
        , IDbCanSearch<PaymentType>
        , IDbCanUpdate<PaymentType>
        , IDbCanUpdateInTransaction<PaymentType>
    {
    }
}
