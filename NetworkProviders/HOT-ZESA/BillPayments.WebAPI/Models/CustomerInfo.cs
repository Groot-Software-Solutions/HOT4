namespace BillPayments.WebAPI.Models
{
    public class CustomerInfo : BaseResponse
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string MeterNumber { get; set; }
        public string Currency { get; set; }
    }
}
