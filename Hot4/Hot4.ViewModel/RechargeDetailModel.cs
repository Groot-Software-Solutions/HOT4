using System.ComponentModel.DataAnnotations.Schema;

namespace Hot4.ViewModel
{
    public class RechargeDetailModel
    {
        public long RechargeId { get; set; }
        public byte StateId { get; set; }
        public string State { get; set; }
        public long AccessId { get; set; }
        public string AccessCode { get; set; }
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }
        [Column(TypeName = "money")]
        public decimal Discount { get; set; }
        public string Mobile { get; set; }
        public byte BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandSuffix { get; set; }
        public byte NetworkId { get; set; }
        public string Network { get; set; }
        public string NetworkPrefix { get; set; }
        public DateTime RechargeDate { get; set; }
        public DateTime? InsertDate { get; set; }

    }
}
