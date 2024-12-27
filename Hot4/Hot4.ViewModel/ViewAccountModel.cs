namespace Hot4.ViewModel
{
    public class ViewAccountModel
    {
        public long AccountId { get; set; }
        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public string AccountName { get; set; }
        public string NationalId { get; set; }
        public string EmailId { get; set; }
        public string RefferedBy { get; set; }

        public decimal Balance { get; set; }
        public decimal SaleValue { get; set; }
        public decimal ZESABalance { get; set; }
        public decimal USDBalance { get; set; }
        public decimal USDUtilityBalance { get; set; }

    }
}
