namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IBundles: IDbContextTable<Bundle>
        , IDbCanList<Bundle>
        , IDbCanGetById<Bundle>
    {
    }
}
