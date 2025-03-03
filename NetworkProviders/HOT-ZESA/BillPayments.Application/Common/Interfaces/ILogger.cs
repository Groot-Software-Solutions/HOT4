using BillPayments.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Application.Common.Interfaces
{
    public interface ILogger
    {
        public void Save<T>(T data);
        public void Save(ZesaLogItem data);
    }
}
