using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Domain.Models
{
    public class AccountBalance
    { 
            public int MerchantId { get; set; }
            public int Service { get; set; }
            public bool Active { get; set; }
            public decimal Balance { get; set; }
            public decimal CreditLimit { get; set; }
            public string Currency { get; set; } 
    }


}
