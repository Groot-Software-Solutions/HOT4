namespace Hot4.Core.DataViewModels
{
    public class AccountSearchModel
    {

        public long AccountID { get; set; }
        public int ProfileID { get; set; }
        public string ProfileName { get; set; }
        public string AccountName { get; set; }
        public string NationalID { get; set; }

        public string Email { get; set; }

        public string ReferredBy { get; set; }

        public decimal Balance { get; set; }
        public decimal SaleValue { get; set; }
        public decimal ZESABalance { get; set; }
        public decimal USDBalance { get; set; }
        public decimal USDUtilityBalance { get; set; }
    }
}
