using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillPayments.WebAPI.Models
{
    public class PurchaseTokenRequest
    {
        public string MeterNumber { get; set; } 
        public decimal Amount { get; set; }
        public string Reference { get; set; }

    }
}
