namespace Hot4.ViewModel
{
    public class BundleRecord
    {
        public int BundleId { get; set; }
        public byte BrandId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Amount { get; set; }
        public required string ProductCode { get; set; }
        public int? ValidityPeriod { get; set; }
        public bool Enabled { get; set; }
    }
}
