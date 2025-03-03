using BillPayments.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Models
{
    public class PaymentRequest :BaseRequest
    {
        public decimal TransactionAmount { get; set; }
        public string SourceMobile { get; set; }
        public string UtilityAccount { get; set; }
        public string ProductName { get; set; }
        public string MerchantName { get; set; }
        public string CurrencyCode { get; set; }
        public new string ProcessingCode { get; set; } = ProcessingCodes.PaymentRequest;
        public string RequiresVoucher { get; set; }
    }
}
