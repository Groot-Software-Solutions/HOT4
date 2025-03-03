using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillPayments.Domain.Models
{
    public class CustomerInfoResponse : BaseResponse
    { 
        public string TransactionAmount { get; set; }
        public new string TransmissionDate { get; set; } 
        public string MerchantName { get; set; }
        public string CustomerData { get; set; }
        public string CustomerAddress { get; set; }
        public string ProductName { get; set; }
        public string UtilityAccount { get; set; }

        // COH
        public bool RequiresVoucher { get; set; }
        public string SubProductName { get; set; }
        public string Arrears { get; set; }

    }
}
