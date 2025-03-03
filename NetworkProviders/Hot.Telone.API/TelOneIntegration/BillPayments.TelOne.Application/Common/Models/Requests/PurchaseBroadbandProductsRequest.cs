using TelOne.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelOne.Application.Common.Models
{
    public class PurchaseBroadbandProductsRequest : Request
    { 
        public string MerchantReference { get; set; }
        public List<OrderProduct> OrderProducts { get; set; }
        public string Currency { get; set; }
    }
}
