namespace Hot4.ViewModel
{
    public class BrandRecord
    {
        public byte BrandId { get; set; }
        public byte NetworkId { get; set; }
        public required string BrandName { get; set; }
        public required string BrandSuffix { get; set; }
        public int? WalletTypeId { get; set; }
    }
}
