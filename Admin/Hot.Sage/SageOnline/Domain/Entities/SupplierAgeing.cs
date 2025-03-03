using System;
using System.Collections.Generic;

namespace Sage.Domain.Entities
{
    public class SupplierAgeing
    {
        public Supplier Supplier { get; set; } = new Supplier();
        public DateTime Date { get; set; }
        public List<AgeingTransaction> AgeingTransactions { get; set; } = new List<AgeingTransaction>();
    }


}
