using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables;

public class ProductMetaDataTypes : Table<ProductMetaDataType>, IProductMetaDataTypes
{
    public ProductMetaDataTypes(IDbHelper dbHelper) : base(dbHelper)
    {
        base.StoredProcedurePrefix = "x";
        base.ListSuffix = "_List";
    }

}