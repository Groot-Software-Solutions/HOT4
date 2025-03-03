using Domain.DataModels;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class Logger : ILogger
    {
        readonly IDbContext _context;

        public Logger(IDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LogData(LogItem logitem)
        {
            return await _context.LogData_Save(logitem);
        }

        public async Task<bool> LogWebItem(WebAPILog logitem)
        { 
            return await _context.WhatsAppLog_Save(logitem); 
        }

    }
}
