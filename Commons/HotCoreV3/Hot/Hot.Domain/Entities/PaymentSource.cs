using System;
using System.Collections.Generic; 
using System.Text;

namespace Hot.Domain.Entities
{
    public class PaymentSource
    { 
        public byte PaymentSourceId { get; set; } 
        public string PaymentSourceText { get; set; } = string.Empty;
        public int WalletTypeId { get; set; }
    }
}
