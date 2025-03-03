namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ISMPPs : IDbContextTable<SMPP>
        , IDbCanAdd<SMPP>
        , IDbCanAddInTransaction<SMPP>
        , IDbCanGetById<SMPP>
        , IDbCanList<SMPP>
        , IDbCanRemoveById<SMPP>
        , IDbCanSearch<SMPP>
        , IDbCanUpdate<SMPP>
        , IDbCanUpdateInTransaction<SMPP>
    {
    }
}
