using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Enums
{
    public enum HotPaymentTypes
    {
        Cash = 1
        , Reversal = 2
        , CreditAdvanced = 3
        , CreditRepayment = 4
        , Freebie = 5
        , HOTTransfer = 6
        , BankManual = 7
        , BankAuto = 8
        , CommissionPaid = 9
        , Depositfees = 10
        , zService_Fees = 11
        , zBalance_BF = 12
        , ShareholdersAllowance = 13
        , Writeoff = 14
        , Direct = 15
        , ZESA = 16
        , USD = 17
        , Nyaradzo = 18
        , USDUtility = 19                   //KMR 04feb2025 added
        , TradeIn = 20                      //KMR 04feb2025
        , StockAccountTransfer = 21         //KMR 04feb2025
        , NetworkRechargeReversal = 22      //KMR 04feb2025
    }
}
