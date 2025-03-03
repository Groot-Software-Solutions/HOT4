using System.Collections.Generic;

namespace Hot.API.Client.Models
{
    public class ZESARechargeResponse : Response
    {
        public decimal WalletBalance { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string Meter { get; set; } 
        public List<ZesaToken> Tokens { get; set; }
        public int RechargeId { get; set; }
        public ZESACustomerInfo CustomerInfo { get; set; }
    }
}
