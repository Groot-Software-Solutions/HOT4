using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class BankTransactionType
    {
        

        public byte Id { get; set; }
        public string BankTransactionTypeText { get; set; } = string.Empty;
    }
}
