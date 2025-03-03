using System;
using System.Text;

namespace Hot.API.Client.Models
{
    public class ZESARechargeRequest
    {
        public string MeterNumber { get; set; }
        public decimal Amount { get; set; }
        public string TargetNumber { get; set; }
        public string CustomerSMS { get; set; }

    }
}
