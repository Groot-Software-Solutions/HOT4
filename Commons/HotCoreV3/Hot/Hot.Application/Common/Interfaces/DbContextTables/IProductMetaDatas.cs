namespace Hot.Application.Common.Interfaces.DbContextTables;

public interface IProductMetaDatas : IDbContextTable<ProductMetaData>
    , IDbCanList<ProductMetaData>

{
}
