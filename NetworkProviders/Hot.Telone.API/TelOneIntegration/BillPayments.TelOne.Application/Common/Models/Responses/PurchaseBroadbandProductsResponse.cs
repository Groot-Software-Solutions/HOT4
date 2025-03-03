using TelOne.Domain.Models;
using System.Collections.Generic;

namespace TelOne.Application.Common.Models
{
    public class PurchaseBroadbandProductsResponse : Response
    {
        public string MerchantReference { get; set; }
        public string OrderNumber { get; set; }
        public List<Voucher> Vouchers { get; set; }
        public decimal Balance { get; set; }
    }
}
