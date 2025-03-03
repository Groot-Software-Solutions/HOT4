namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IAccessWebs : IDbContextTable<AccessWeb>
        , IDbCanAdd<AccessWeb>
        , IDbCanAddInTransaction<AccessWeb>
        , IDbCanGetById<AccessWeb>
        , IDbCanList<AccessWeb>
        , IDbCanRemoveById<AccessWeb>
        , IDbCanSearch<AccessWeb>
        , IDbCanUpdate<AccessWeb>
        , IDbCanUpdateInTransaction<AccessWeb>
    { 
    }
}
