using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelOne.Application.Common.Models
{
    public class RechargeAdslAccountRequest : Request
    { 
        public string MerchantReference { get; set; }
        public int ProductId { get; set; }
        public string CustomerAccount { get; set; }
        public string Currency { get; set; }
    }
     

}
