using TelOne.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelOne.Application.Common.Interfaces
{
    public interface ILogger
    {
        public void Save<T>(T data);
        public void Save(LogItem data);
    }
}
