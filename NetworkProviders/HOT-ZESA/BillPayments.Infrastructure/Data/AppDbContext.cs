using BillPayments.Application.Common.Models;
using BillPayments.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<BackgroundTask> BackgroundTasks { get; set; }
        public DbSet<ZesaLogItem> ZesaLogs { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        

    }
}
