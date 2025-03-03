using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BillPayments.Domain.Helpers
{
    public static class DateHelper
    {
        public static string BillPaymentFormat(DateTime date) => date.ToString("MMddyyHHmmss");
       
        public static DateTime ParseBillPaymentFormat(string date) => (DateTime)(DateTime
                .TryParseExact(date, "MMddyyHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var dt) 
                ? dt 
                : null as DateTime?);
        
    }
}
