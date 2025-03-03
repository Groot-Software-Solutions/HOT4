namespace TelOne.Domain.Models
{
    public class Voucher
    {
        public int ProductId { get; set; }
        public string PIN { get; set; }
        public string BatchNumber { get; set; }
        public string SerialNumber { get; set; } 
        public string CardNumber { get; set; }
    }
}
