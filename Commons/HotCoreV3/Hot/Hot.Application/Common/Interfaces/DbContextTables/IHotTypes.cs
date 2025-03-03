namespace Hot.Application.Common.Interfaces.DbContextTables
{
    public interface IHotTypes : IDbContextTable<HotType>
        , IDbCanList<HotType>

    {
        public Task<OneOf<int, HotDbException>> IndentifyAsync(string TypeCode, int SplitCount);
    }
}
