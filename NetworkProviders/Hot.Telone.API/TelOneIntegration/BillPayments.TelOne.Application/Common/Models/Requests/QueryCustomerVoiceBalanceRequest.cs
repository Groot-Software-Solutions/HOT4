using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
   public class QueryCustomerVoiceBalanceRequest:Request
    {
        public string MSISDN { get; set; }
    }
}
