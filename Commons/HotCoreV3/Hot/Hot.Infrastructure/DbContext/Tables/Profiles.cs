using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using Hot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.DbContext.Tables
{
    public class Profiles : Table<Profile>, IProfiles
    {
        public Profiles(IDbHelper dbHelper) : base(dbHelper)
        {
            base.StoredProcedurePrefix = "x";
            base.ListSuffix = "_List";
        }
    }
}
