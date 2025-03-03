using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Enums
{
    public enum ResponseCodes
    {
        TransactionSuccessful = 0,
        GeneralError=5,
        TransactionInProgress =9,
        AmountOutsideRange =14,
        InvalidMeterNumber = 14,
        InsufficientFunds =51,
        AccountPendingReversal=57,
        SecurityViolation=63,
        TransactionTimeout=68,
        DuplicateTransaction=94,
         
    }
}
