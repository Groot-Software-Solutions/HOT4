using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Helpers
{
    public static class VendorBalanceHelper
    {
        public static decimal GetBalanceAmount(string data)
        { 
            return Convert.ToDecimal(data.Split("|")[3]) / 100;
        }
    }
}
