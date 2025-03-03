using System.Collections.Generic;
using TelOne.Domain.Models;

namespace TelOne.Application.Common.Models
{
    public class RechargeAdslAccountResponse : Response
    {
        public string MerchantReference { get; set; }
        public string OrderNumber { get; set; }
        public string AccountNumber { get; set; }
        public List<Voucher> Voucher { get; set; }
        public decimal Balance { get; set; }
    }
} 