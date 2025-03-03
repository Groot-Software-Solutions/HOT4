using System.Collections.Generic;

namespace Hot.API.Client.Models
{
    public class TeloneBundlePurchaseResponse : Response
    {
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal WalletBalance { get; set; }
        public int RechargeId { get; set; }
        public List<TeloneVoucher> Vouchers { get; set; }
    }

}
