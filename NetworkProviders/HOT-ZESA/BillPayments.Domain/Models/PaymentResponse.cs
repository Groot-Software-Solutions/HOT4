using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Models
{
    public class PaymentResponse : BaseResponse
    {
        public string TransactionAmount { get; set; }
        public string SourceMobile { get; set; }
        public string UtilityAmount { get; set; }
        public string MerchantName { get; set; }
        public string ProductName { get; set; }
        public string CurrencyCode { get; set; }
        public string ReceiptNumber { get; set; }

        
        public string Arears { get; set; }
        public string PaymentType { get; set; }
        public string Token { get; set; }
        public string FixedCharges { get; set; }
        public string MescellaneousData { get; set; }

        
        public string RequiresVoucher { get; set; }
        public string SubProductName { get; set; }
        public string CustomerData { get; set; }
        public string CustomerAddress { get; set; }

    }
}
