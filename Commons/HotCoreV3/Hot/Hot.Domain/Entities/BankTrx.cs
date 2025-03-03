using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class BankTrx
    {
        public long BankTrxID { get; set; }

        public long BankTrxBatchID { get; set; }

        public byte BankTrxTypeID { get; set; }

        public byte BankTrxStateID { get; set; }

        public decimal Amount { get; set; }

        public DateTime TrxDate { get; set; }

        public string Identifier { get; set; } = string.Empty;

        public string RefName { get; set; } = string.Empty;

        public string Branch { get; set; } = string.Empty;

        public string BankRef { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        public long? PaymentID { get; set; }

    }
}
