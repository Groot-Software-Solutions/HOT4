namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface INetworks : IDbContextTable<Network>,
        IDbCanList<Network>
    {
        public Task<OneOf<int, HotDbException>> IndentifyAsync(string Mobile);
    }
}
