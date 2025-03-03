using Hot.Lib.Entities;

namespace Hot.Domain.Entities
{
    public class EconetReconUSD
    {
        public string? TransactionId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Reference { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public long DebitAmount { get; set; }
        public long CreditAmount { get; set; }

        public long Balance { get; set; }
        public string To { get; set; } = string.Empty;

        public decimal TotalAmount
        {
            get
            {
                if (DebitAmount != 0) return DebitAmount;
                return CreditAmount;
            }
        }

    }
}