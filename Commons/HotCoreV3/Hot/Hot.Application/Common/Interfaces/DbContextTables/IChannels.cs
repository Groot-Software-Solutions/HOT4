namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IChannels : IDbContextTable<Channel>
         , IDbCanList<Channel>
    {
    }
}
