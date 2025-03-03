
namespace Hot.Domain.Entities
{
    public class Account
    {
        public long AccountID { get; set; }

        public int ProfileID { get; set; }

        public string AccountName { get; set; } = string.Empty;

        public string NationalID { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? ReferredBy { get; set; }

        public DateTime? InsertDate { get; set; }

        public decimal Balance { get; set; }

        public decimal SaleValue { get; set; }

        public decimal ZesaBalance { get; set; }

        public decimal USDBalance { get; set; }

        public decimal USDUtilityBalance { get; set; } 
    }
}
