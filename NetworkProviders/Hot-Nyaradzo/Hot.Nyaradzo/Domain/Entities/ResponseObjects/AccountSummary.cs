using Hot.Application.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hot.Nyaradzo.Domain.Entities
{
    public class AccountSummary
    {
        public string id { get; set; }
        public DateTime dateCreated { get; set; } = DateTime.Now;
        public string policyNumber { get; set; }
        public string sourceReference { get; set; }
        public string status { get; set; }
        public string responseCode { get; set; }
        public string monthlyPremium { get; set; }
        public string amountToBePaid { get; set; }
        public string numberOfMonths { get; set; }
        public string responseDescription { get; set; }
        public string balance { get; set; }
        public string policyHolder { get; set; }
        public string bankCode { get; set; }
        public string txnRef { get; set; }
        public string currencyCode { get; set; }
    }

}
