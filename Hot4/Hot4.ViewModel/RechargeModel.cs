namespace Hot4.ViewModel
{
    public class RechargeModel
    {
        public long RechargeId { get; set; }

        public byte StateId { get; set; }

        public long AccessId { get; set; }

        public decimal Amount { get; set; }

        public decimal Discount { get; set; }

        public required string Mobile { get; set; }

        public byte BrandId { get; set; }

        public DateTime RechargeDate { get; set; }

        public DateTime? InsertDate { get; set; }
        // zesa
        public bool IsSuccessFul { get; set; }
        // telone 
        public decimal Denomination { get; set; }
        public long Quantity { get; set; }

    }
}
