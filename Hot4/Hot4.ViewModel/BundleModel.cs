namespace Hot4.ViewModel
{
    public class BundleModel
    {
        public int BundleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
        public string ProductCode { get; set; }
        public int? ValidityPeriod { get; set; }
        public bool Enabled { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Network { get; set; }
    }
}
