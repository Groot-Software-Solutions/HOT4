using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class HotTypes : Table<HotType>, IHotTypes
    {
        public HotTypes(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.ListSuffix = "_List";
        }

        public async Task<OneOf<int, HotDbException>> IndentifyAsync(string TypeCode, int SplitCount)
        {
            return await dbHelper.QuerySingle<int>($"{GetSPPrefix()}_Identify @TypeCode,@SplitCount", new { TypeCode, SplitCount });
        }
        public OneOf<int, HotDbException> Indentify(string TypeCode, int SplitCount)
        {
            return IndentifyAsync(TypeCode, SplitCount).Result;
        }
    }

}
