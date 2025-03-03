namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface INetworkBalances :IDbContextTable<NetworkBalance>
        ,IDbCanList<NetworkBalance>

    {
        public Task<OneOf<NetworkBalance, HotDbException>> GetByIdAsync(int BrandId);
    }
}
