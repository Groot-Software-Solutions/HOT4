using System;
using System.Collections.Generic;

namespace Sage.Domain.Entities
{
    public class CustomerAgeing
    {
        public DateTime Date { get; set; }
        public List<AgeingTransaction> AgeingTransactions { get; set; }
    }


}
