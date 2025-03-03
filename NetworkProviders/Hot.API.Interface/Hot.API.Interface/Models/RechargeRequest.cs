using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Models
{
    public class RechargeRequest
    {
        public decimal Amount;
        public string TargetMobile;
        public int BrandID;
        public string CustomerSMS;

    }
}
