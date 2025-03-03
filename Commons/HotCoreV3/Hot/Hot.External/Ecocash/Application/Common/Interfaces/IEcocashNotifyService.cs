using Hot.Domain.Entities;
using Hot.Ecocash.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Ecocash.Application.Common.Interfaces
{
    public interface IEcocashNotifyService
    {
        public Task CheckPendingTransactions();
        public Task HandlePendingTransaction(BankTrx bankTrx, Transaction transaction);
    }
}
