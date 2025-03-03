using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class BankTransactionState
    {
        public byte Id { get; set; }
        public string BankTransactionStateText { get; set; } = string.Empty;
    }
}
