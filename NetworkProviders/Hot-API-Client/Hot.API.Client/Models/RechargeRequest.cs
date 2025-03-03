using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class RechargeRequest
    {
        public decimal Amount { get; set; }=decimal.Zero;   
        public string TargetMobile { get; set; } =String.Empty;
        public int? BrandID { get; set; }
        public string? CustomerSMS { get; set; }

    }
}
