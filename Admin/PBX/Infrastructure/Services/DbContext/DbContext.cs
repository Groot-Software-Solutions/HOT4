using Application.Common.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services.DbContext
{
    public class DbContext : IDbContext
    {
        public DbContext(IDbContextTable<SMS> sMSs)
        {
            SMSs = sMSs;
        }

        public IDbContextTable<SMS> SMSs { get; set; }
    }
}
