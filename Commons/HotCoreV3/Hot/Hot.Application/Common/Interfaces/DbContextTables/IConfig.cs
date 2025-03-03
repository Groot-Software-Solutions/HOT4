namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IConfigs : IDbContextTable<Config>
    {
        public Task<OneOf<Config, HotDbException>> SelectAsync();
    }
}
