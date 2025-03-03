using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Models
{
    public class VendorBalanceRequest : BaseRequest
    { 
        public string TransactionReference { get; set; }
        public string ResponseCode { get; set; }
        public string VendorBalance { get; set; }
        public string Narrative { get; set; }
        public string AccountNumber { get; set; }
        public string CurrencyCode { get; set; } 
        public new string ProcessingCode { get; set; } = ProcessingCodes.VendorBalanceEnquiry;

    }
}
