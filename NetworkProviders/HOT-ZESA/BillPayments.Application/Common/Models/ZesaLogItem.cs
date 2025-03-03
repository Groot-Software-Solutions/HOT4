using BillPayments.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace BillPayments.Application.Common.Models
{
    public class ZesaLogItem
    {
        public ZesaLogItem()
        {
        }

        public ZesaLogItem(PurchaseTokenRequest request, PurchaseTokenResponse response)
        {
            Request = JsonSerializer.Serialize(request);
            Response = JsonSerializer.Serialize(response);
            Date = DateTime.Now;
            HotReference = request.VendorReference;
        }

        public int Id { get; set; }
        public string HotReference { get; set; }
        public string Request { get; set; }
        public string  Response { get; set; }
        public DateTime Date { get; set; }

    }
}

