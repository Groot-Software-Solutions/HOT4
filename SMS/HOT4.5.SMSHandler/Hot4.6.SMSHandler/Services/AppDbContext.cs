using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.DbContextTables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Infrastructure.Services
{
    public class AppDbContext : IDbContext
    {
        public AppDbContext(ITemplates templates)
        {
            Templates = templates;
        }

        public ITemplates Templates { get; set; }
        public ISMSs SMSs { get; set; }
    }
}
