using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Model.PurchaseToken
{

    public class PurchaseToken 
        {

            public string Reference { get; set; }
            public DateTime Date { get; set; }
            public decimal Amount { get; set; }
            public string MeterNumber { get; set; }
            public string RawResponse { get; set; }
            public string ResponseCode { get; set; }
            public string Narrative { get; set; }
            public string VendorReference { get; set; }
            public List<TokenItem> Tokens { get; set; }
        }
    
}
