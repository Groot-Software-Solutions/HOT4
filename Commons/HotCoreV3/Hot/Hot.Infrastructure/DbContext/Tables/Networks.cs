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
    public class Networks : Table<Network>, INetworks
    {
        public Networks(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }

        public async Task<OneOf<int, HotDbException>> IndentifyAsync(string Mobile)
        {
            return await dbHelper.QuerySingle<int>($"{GetSPPrefix()}_Identify @Mobile", new { Mobile });
        }
        public OneOf<int, HotDbException> Indentify(string Mobile)
        {
            return IndentifyAsync(Mobile).Result;
        }
    }
}
