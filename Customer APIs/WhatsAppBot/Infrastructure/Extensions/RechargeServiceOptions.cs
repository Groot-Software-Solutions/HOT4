using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Extensions
{
    public class RechargeServiceOptions
    {
        public string BaseUrl { get; set; }
        public string AccessCode { get; set; }
        public string AccessPassword { get; set; }
    }
}
