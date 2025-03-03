namespace Hot.Application.Common.Models
{
    public class PinStockModel
    {
        public int Brandid { get; set; }
        public string BrandName { get; set; } = string.Empty;
        public decimal PinValue { get; set; }
        public decimal Stock { get; set; }
    }
}
