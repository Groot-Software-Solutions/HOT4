using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Application.Common.Models
{
    public class QueryMerchantTransactionsRequest : Request
    {
        public DateTime startDate { get; set; } = DateTime.Now.AddDays(-1);
        public DateTime endDate { get; set; } = DateTime.Now;
    }
}
