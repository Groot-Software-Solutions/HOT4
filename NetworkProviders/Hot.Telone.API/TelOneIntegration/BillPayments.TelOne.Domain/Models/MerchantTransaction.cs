using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Domain.Models
{
    public class MerchantTransaction
    { 
        public string OrderNumber { get; set; }
        public string MerchantReference { get; set; }
        public decimal Amount { get; set; }
        public string ResultCode { get; set; }
        public string Status { get; set; }
        public DateTime TransactionDatetime { get; set; } 
    }
}
