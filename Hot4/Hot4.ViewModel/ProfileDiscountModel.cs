using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.ViewModel
{
    public class ProfileDiscountModel
    {
        public int ProfileDiscountId { get; set; }
        public int ProfileId { get; set; }
        public byte BrandId { get; set; }
        public int NetworkId { get; set; }
        public string Network { get; set; }
        public string NetworkPrefix { get; set; }
        public string BrandName { get; set; }
        public string BrandSuffix { get; set; }
        [Column(TypeName = "money")]
        public decimal Discount { get; set; }
    }
}
