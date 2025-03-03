using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Helpers
{
    public static class AmountHelper
    {
        public static int BillPaymentAmount(decimal amount) => (int)(amount*100) ;
        public static decimal ActualAmount(int amount)
        {
            if (amount == 0) return 0;
            return ((decimal)amount) / 100;
        } 
        public static decimal ActualAmount(string amount)
        { 
            if (string.IsNullOrEmpty(amount)) return 0;
            try
            {   
                return ActualAmount(Convert.ToInt32(amount)); 
            }
            catch (Exception)
            {
                return 0;
            } 
        }
    }
}
