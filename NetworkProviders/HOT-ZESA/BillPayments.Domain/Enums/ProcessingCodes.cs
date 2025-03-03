using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Enums
{
    public class ProcessingCodes
    {
        public string Value { get; set; }
        private ProcessingCodes(string value) { Value = value; }
        public static implicit operator string(ProcessingCodes item) => item.Value;

        public static ProcessingCodes VendorBalanceEnquiry => new ProcessingCodes("300000"); 
        public static ProcessingCodes CustomerInformation => new ProcessingCodes("310000"); 
        public static ProcessingCodes LastCustomerToken => new ProcessingCodes("320000"); 
        public static ProcessingCodes TokenPurchaseRequest => new ProcessingCodes("U50000"); // Can be also used for Resend
        public static ProcessingCodes PaymentRequest => new ProcessingCodes("520000");

        

    }
}
