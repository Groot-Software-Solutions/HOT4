using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.WebAPI.Models
{
    public class Payment : BaseResponse
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Arreas { get; set; }
        public decimal Levy { get; set; }
        public decimal OtherCharges { get; set; }
        public string MeterNumber { get; set; }
        public string Token { get; set; }
        public string RawResponse { get; set; }
        public string ResponseCode { get; set; }
        public string Narrative { get; set; }
        public string VendorReference { get; internal set; }
        public string ReceiptNumber { get; set; }
    }
}
