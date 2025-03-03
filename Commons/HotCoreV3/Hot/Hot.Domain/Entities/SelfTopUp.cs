using Hot.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class SelfTopUp
    {
        public long SelfTopUpId { get; set; }   
        
        public long AccessId { get; set; }
        public long? BanktrxId { get; set; }
        public long? RechargeId { get; set; }
        public int BrandId { get; set; }
        public int StateId { get; set; }


        public string TargetNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string? ProductCode { get; set; } = null;
        public string? NotificationNumber { get; set; }


        public string BillerNumber { get; set; } = string.Empty ;
        public DateTime InsertDate { get; set; }
        public Currency Currency { get; set; } = Currency.ZWG;

    }
}
