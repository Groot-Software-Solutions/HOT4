using BillPayments.Domain.Model.PurchaseToken;
using System;
using System.Collections.Generic;
using System.Text; 

namespace BillPayments.Application.Common.Models
{
    public class HotRetyUpdate
    {
        public int RechargeId { get; set; }
        public PurchaseToken PurchaseToken { get; set; }
    }
}
