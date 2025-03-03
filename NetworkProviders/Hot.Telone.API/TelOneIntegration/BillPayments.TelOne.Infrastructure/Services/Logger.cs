using TelOne.Application.Common.Interfaces;
using TelOne.Application.Common.Models;
using TelOne.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelOne.Infrastructure.Services
{
    public class Logger : ILogger
    {
        readonly AppDbContext _dbContext;

        public Logger(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save<T>(T data)
        {
            try
            {
                //_dbContext.ZesaLogs.Add(data);
            }
            catch (Exception)
            {

            }
        }

        public void Save(LogItem data)
        {
            try
            {
                _dbContext.ZesaLogs.Add(data);
                _dbContext.SaveChanges();
            }
            catch (Exception)
            {

            }

        }
    }
}
