using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.DbContext.Tables
{
   public class AccessWebs : Table<AccessWeb>, IAccessWebs
    {
        public AccessWebs(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x"; 
            base.AddSuffix = "_Save";
            base.GetSuffix = "_Select";
            base.UpdateSuffix = "_Save";
            base.IdPrefix = "Access";
        }
    }
}
