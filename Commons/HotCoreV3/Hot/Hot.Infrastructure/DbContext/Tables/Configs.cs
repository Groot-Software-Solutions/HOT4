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
    public class Configs : Table<Config>, IConfigs
    {
        public Configs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
        }

        public async Task<OneOf<Config, HotDbException>> SelectAsync()
        {
            return await dbHelper.QuerySingle<Config>($"{GetSPPrefix()}_Select", new { });
        }
    }
}
