using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Models
{
    public class BaseResponse
    {
        public string Mti { get; set; }
        public string VendorReference { get; set; }
        public string ProcessingCode { get; set; }
        public string TransmissionDate { get; set; }
        public string VendorNumber { get; set; }
        public string TransactionReference { get; set; }
        public string Narrative { get; set; }
        public string ResponseCode { get; set; }
        public string CurrencyCode { get; set; }
    }
}
