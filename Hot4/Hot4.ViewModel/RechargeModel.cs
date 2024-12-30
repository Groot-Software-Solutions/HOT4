namespace Hot4.ViewModel
{
    public class RechargeModel
    {
        public long RechargeId { get; set; }
        public byte StateId { get; set; }
        public long AccessId { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string Mobile { get; set; }
        public DateTime RechargeDate { get; set; }
        public byte BrandId { get; set; }
    }
}
