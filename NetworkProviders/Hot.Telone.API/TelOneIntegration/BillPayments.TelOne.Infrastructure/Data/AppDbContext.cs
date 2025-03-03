using TelOne.Application.Common.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelOne.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<LogItem> ZesaLogs { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
    }
}
