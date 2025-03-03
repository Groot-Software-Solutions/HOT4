namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IPaymentSources  : IDbContextTable<PaymentSource>
        , IDbCanAdd<PaymentSource>
        , IDbCanAddInTransaction<PaymentSource>
        , IDbCanGetById<PaymentSource>
        , IDbCanList<PaymentSource>
        , IDbCanRemoveById<PaymentSource>
        , IDbCanSearch<PaymentSource>
        , IDbCanUpdate<PaymentSource>
        , IDbCanUpdateInTransaction<PaymentSource>
    {
    }
}
