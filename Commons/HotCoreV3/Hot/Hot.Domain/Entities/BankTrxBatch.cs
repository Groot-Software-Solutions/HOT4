using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{ 

    public partial class BankTrxBatch
    {
        public long BankTrxBatchID { get; set; }
        public int BankID { get; set; } 
        public DateTime BatchDate { get; set; }
        public string BatchReference { get; set; } = string.Empty;
        public string LastUser { get; set; } = string.Empty;
    }
}
