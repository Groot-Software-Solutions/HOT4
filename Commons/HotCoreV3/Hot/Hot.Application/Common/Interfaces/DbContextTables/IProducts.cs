namespace Hot.Application.Common.Interfaces.DbContextTables;

public interface IProducts : IDbContextTable<Product>
    , IDbCanList<Product>

{
}
