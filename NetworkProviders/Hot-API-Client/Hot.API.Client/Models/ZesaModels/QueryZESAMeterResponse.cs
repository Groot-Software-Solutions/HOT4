namespace Hot.API.Client.Models
{
    public class QueryZESAMeterResponse : Response
    {
        public string Meter { get; set; }
        public ZESACustomerInfo CustomerInfo { get; set; }
    }
}
