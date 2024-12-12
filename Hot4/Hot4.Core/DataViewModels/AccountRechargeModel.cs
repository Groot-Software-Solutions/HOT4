using System.ComponentModel;

namespace Hot4.Core.DataViewModels
{
    public class AccountRechargeModel
    {
        [DisplayName("RechargeID")]
        public long RechargeID { get; set; }

        [DisplayName("State")]
        public string State { get; set; }

        [DisplayName("Access Channel")]
        public string AccessChannel { get; set; }

        [DisplayName("Access Code")]
        public string AccessCode { get; set; }

        [DisplayName("Amount")]
        public decimal Amount { get; set; }

        [DisplayName("Discount")]
        public decimal Discount { get; set; }

        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        [DisplayName("Product")]
        public string ProductName { get; set; }

        [DisplayName("Recharge Date")]
        public DateTime? RechargeDate { get; set; }

        [DisplayName("IsSuccessFul")]
        public bool IsSuccessFul { get; set; }
    }

    public record AccountRechargeRecord
    {
        public long RechargeID { get; set; }
        public byte StateID { get; set; }
        public long AccessID { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public string Mobile { get; set; }
        public int ProductID { get; set; }
        public DateTime RechargeDate { get; set; }
        public DateTime? InsertDate { get; set; }
    }

    public record RechargeAccess
    {
        [DisplayName("RechargeID")]
        public long RechargeID { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Access Channel")]
        public string AccessChannel { get; set; }
        [DisplayName("Access Code")]
        public string AccessCode { get; set; }
        [DisplayName("Amount")]
        public decimal Amount { get; set; }
        [DisplayName("Discount")]
        public decimal Discount { get; set; }
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
        [DisplayName("Brand")]
        public string BrandName { get; set; }
        [DisplayName("Recharge Date")]
        public DateTime? InsertDate { get; set; }
        [DisplayName("IsSuccessFul")]
        public bool IsSuccessFul { get; set; }
        public long AccessID { get; set; }
    }
}
