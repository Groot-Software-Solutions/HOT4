using BillPayments.Application.Common.Interfaces;
using BillPayments.Application.Common.Models;
using BillPayments.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Infrastructure.Services
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

        public void Save(ZesaLogItem data)
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
