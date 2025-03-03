using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables;

public class Brands : Table<Brand>, IBrands
{
    public Brands(IDbHelper dbHelper) : base(dbHelper)
    {
        base.StoredProcedurePrefix = "x";
        base.ListSuffix = "_List";
        base.GetSuffix = "_Get";
    }

    public async Task<OneOf<int, HotDbException>> IndentifyAsync(int NetworkId, string BrandSuffix)
    {
        return await dbHelper.QuerySingle<int>($"{GetSPPrefix()}_Identify @NetworkId,@BrandSuffix", new { NetworkId, BrandSuffix });
    }

    public OneOf<int, HotDbException> Indentify(int NetworkId, string BrandSuffix)
    {
        return IndentifyAsync(NetworkId, BrandSuffix).Result;
    }
}
