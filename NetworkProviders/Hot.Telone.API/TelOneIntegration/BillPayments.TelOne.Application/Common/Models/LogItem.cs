
using System;
using System.Text.Json; 
namespace TelOne.Application.Common.Models
{
    public class LogItem
    {
        public LogItem()
        {
        }

        public LogItem(PurchaseBroadbandProductsRequest request, PurchaseBroadbandProductsResponse response)
        {
            Request = JsonSerializer.Serialize(request);
            Response = JsonSerializer.Serialize(response);
            Date = DateTime.Now;
            HotReference = request.MerchantReference;
        }

        public int Id { get; set; }
        public string HotReference { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public DateTime Date { get; set; }

    }
}
