using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Models
{
    public class PurchaseTokenResponse : BaseResponse
    {
        
        public string TransactionAmount { get; set; } 
        public string Arrears { get; set; }
        public string UtilityAccount { get; set; } 
        public string PaymentType { get; set; }
        public string Token { get; set; }
        public string FixedCharges { get; set; }
        public string MiscellaneousData { get; set; }
        public string CurrencyCode { get; set; }
        public string MerchantName { get; set; }
        public string ProductName { get; set; }
    }
}
