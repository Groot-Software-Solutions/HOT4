namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface ITemplates : IDbContextTable<Template>
        , IDbCanAdd<Template>
        , IDbCanAddInTransaction<Template>
        , IDbCanGetById<Template>
        , IDbCanList<Template>
        , IDbCanRemoveById<Template>
        , IDbCanSearch<Template>
        , IDbCanUpdate<Template>
        , IDbCanUpdateInTransaction<Template>
    {
    }
}
