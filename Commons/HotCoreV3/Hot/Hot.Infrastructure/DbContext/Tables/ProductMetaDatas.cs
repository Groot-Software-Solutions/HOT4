using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables;

public class ProductMetaDatas : Table<ProductMetaData>, IProductMetaDatas
{
    public ProductMetaDatas(IDbHelper dbHelper) : base(dbHelper)
    {
        base.StoredProcedurePrefix = "x";
        base.ListSuffix = "_List";
    }

}
