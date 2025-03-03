using Hot.Ecocash.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Ecocash.Domain.Entities
{
    public class ChargingInformation
    {
        public decimal amount { get; set; } = 0m;
        public string description { get; set; } = "";
        public string currency { get; set; } = "";
    }
}
