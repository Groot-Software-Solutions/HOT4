using Domain.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILogger
    {
        public Task<bool> LogWebItem(WebAPILog logitem);
        public Task<bool> LogData(LogItem logitem);

    }
}
