using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Logs : Table<Log>, ILogs
    {
        public Logs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.AddSuffix = "_Insert";
            base.AddParameters = "@LogModule,@LogObject,@LogMethod,@LogDescription,@IDTypel,@IDNumber";
        }
    }
}
