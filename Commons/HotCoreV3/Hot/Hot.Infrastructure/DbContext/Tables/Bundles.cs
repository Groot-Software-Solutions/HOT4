using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Interfaces; 
using Hot.Domain.Entities;

namespace Hot.Infrastructure.DbContext.Tables;
public class Bundles : Table<Bundle>, IBundles
{
    public Bundles(IDbHelper dbHelper) : base(dbHelper)
    {
        base.StoredProcedurePrefix = "x";
        base.ListSuffix = "s_List";
        base.GetSuffix = "s_Get";
    }
}
