namespace Hot.Application.Common.Models
{
    public class ProfileDiscountResult
    {
        public string ProfileName { get; set; } = string.Empty;
        public int ProfileDiscountID    { get; set; }
        public int ProfileId { get; set; }
        public int BrandID { get; set; }
        public decimal Discount { get; set; } 
        public int NetworkID { get; set; } 
        public string Network { get; set; } = string.Empty;
        public string BrnadName { get; set; } = string.Empty;
    }
}
