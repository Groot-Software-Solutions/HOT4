using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Models
{
    public class VendorBalanceResponse :BaseResponse
    { 
        public string VendorBalance { get; set; } 
        public string AccountNumber { get; set; }
        public string CurrencyCode { get; set; }
    }
}
