using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Enums
{
    public enum BankTransactionStates
    {
        Pending = 0,
        Success = 1,
        Suspended = 2,
        Ignore = 3,
        Expired = 4,
        Failed = 5,
        BusyConfirming = 6,
        Confirmed = 7,
        Rejected = 8,
        ToBeAllocated = 9,
    }
}
