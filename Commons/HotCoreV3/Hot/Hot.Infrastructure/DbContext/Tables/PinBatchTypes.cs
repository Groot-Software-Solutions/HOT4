using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class PinBatchTypes : Table<PinBatchType>, IPinBatchTypes
    {
        public PinBatchTypes(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            

        }

    }
}
