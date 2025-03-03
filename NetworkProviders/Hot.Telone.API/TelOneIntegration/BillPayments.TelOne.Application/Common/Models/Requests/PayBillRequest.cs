using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
    public class PayBillRequest : Request
    {
        public decimal BillAmount { get; set; }
        //public string PaymentMethodData { get; set; }
        public string PaymentMethod { get; set; }
        public string MerchantReference { get; set; }
        public string Identifier { get; set; }

        public string Currency { get; set; }
        public string Reason { get; set; }

    }
}
