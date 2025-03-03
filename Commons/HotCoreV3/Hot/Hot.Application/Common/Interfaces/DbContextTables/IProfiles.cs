namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IProfiles : IDbContextTable<Profile>
        , IDbCanList<Profile>
    {
    }
}
