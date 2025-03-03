namespace Hot.Application.Common.Interfaces.DbContextTables;

public interface IBrands : IDbContextTable<Brand>
    , IDbCanList<Brand>
    , IDbCanGetById<Brand>

{
    public Task<OneOf<int, HotDbException>> IndentifyAsync(int NetworkId, string BrandSuffix);
}
