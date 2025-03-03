namespace BillPayments.WebAPI.Models
{
    public class VendorBalance :BaseResponse
    {
        public string Balance { get; set; }
        public string AccountNumber { get; set; }
        public string CurrencyCode { get; set; }
        
    }
}
