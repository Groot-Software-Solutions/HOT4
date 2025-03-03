using BillPayments.Domain.Enums;
using BillPayments.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Models
{
    public class BaseRequest
    {
        public string Mti { get; set; } = MessageTypeIndicator.TransactionRequest;
        public string VendorReference { get; set; }
        public string ProcessingCode { get; set; }
        public string TransmissionDate { get; set; } = DateHelper.BillPaymentFormat(DateTime.Now);
        public string VendorNumber { get; set; } 
        public string APIVersion { get; set; } = "02";
    }
}
