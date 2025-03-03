using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SelfTopUp
    {
        public string TargetMobile { get; set; }
        public string BillerMobile { get; set; }
        public decimal Amount { get; set; }

    }
}
