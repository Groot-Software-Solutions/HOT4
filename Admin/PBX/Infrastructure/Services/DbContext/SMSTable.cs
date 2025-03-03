using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.DbContext
{
    public class SMSTable : DbContextTable<SMS>
    {
        public SMSTable(IDbHelper dbHelper) : base(dbHelper)
        {
        }
    }
}
