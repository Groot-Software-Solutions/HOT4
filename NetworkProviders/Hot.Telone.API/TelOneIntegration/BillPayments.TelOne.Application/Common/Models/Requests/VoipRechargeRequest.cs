using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
    public class VoipRechargeRequest : Request
    {
        public string MerchantReference { get; set; }
        public decimal VoiceAmount { get; set; }
        public string Currency { get; set; }
        public string CustomerAccount { get; set; }  
    }
}
