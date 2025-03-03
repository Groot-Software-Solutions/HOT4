namespace Hot.Application.Common.Interfaces.DbContextTables
{

    public interface IStatistics 

    {
        public Task<OneOf<List<StatResult>, HotDbException>> GetSmsTrafficAsync();
        public Task<OneOf<List<StatResult>, HotDbException>> GetRechargeTrafficAsync();
        public Task<OneOf<List<StatResult>, HotDbException>> GetRechargeTrafficLastDayAsync();
    }
}
