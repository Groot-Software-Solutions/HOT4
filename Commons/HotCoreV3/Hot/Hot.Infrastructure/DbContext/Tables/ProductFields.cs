using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables;

public class ProductFields : Table<ProductField>, IProductFields
{
    public ProductFields(IDbHelper dbHelper) : base(dbHelper)
    {
        base.StoredProcedurePrefix = "x";
        base.ListSuffix = "_List"; 
    }
     
}
