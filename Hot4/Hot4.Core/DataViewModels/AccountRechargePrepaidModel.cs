using System.ComponentModel;

namespace Hot4.Core.DataViewModels
{
    public class AccountRechargePrepaidModel
    {
        [DisplayName("RechargeID")]
        public long RechargeID { get; set; }
        [DisplayName("Debit/Credit")]
        public string DebitCredit { get; set; }
        [DisplayName("Return Code ")]
        public string ReturnCode { get; set; }
        [DisplayName("Narrative ")]
        public string Narrative { get; set; }
        [DisplayName("Initial Balance")]
        public decimal InitialBalance { get; set; }
        [DisplayName("Final Balance")]
        public decimal FinalBalance { get; set; }
        [DisplayName("Reference ")]
        public string Reference { get; set; }
        [DisplayName("Initial Wallet")]
        public decimal? InitialWallet { get; set; }
        [DisplayName("Final Wallet")]
        public decimal? FinalWallet { get; set; }
        [DisplayName("Window")]
        public DateTime? Window { get; set; }
        [DisplayName("Data")]
        public string Data { get; set; }
        [DisplayName("SMS")]
        public string SMS { get; set; }
    }
}
