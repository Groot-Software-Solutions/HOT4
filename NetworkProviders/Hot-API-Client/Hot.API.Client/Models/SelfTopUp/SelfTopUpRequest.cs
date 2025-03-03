using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.API.Client.Models
{
    public class SelfTopUpRequest
    {
        public string TargetMobile { get; set; } = string.Empty;
        public string BillerMobile { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;

    }
}
