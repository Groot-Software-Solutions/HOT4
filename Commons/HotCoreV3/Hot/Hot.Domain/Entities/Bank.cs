using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Bank
    {
        public byte BankID { get; set; }

        public string Name { get; set; } = string.Empty;

        public int? SageBankID { get; set; }

    }
}
