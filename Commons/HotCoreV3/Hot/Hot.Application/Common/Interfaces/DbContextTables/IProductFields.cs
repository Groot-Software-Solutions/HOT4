using Hot.Domain.Entities;
namespace Hot.Application.Common.Interfaces.DbContextTables;

public interface IProductFields : IDbContextTable<ProductField>
    , IDbCanList<ProductField>

{
}
