namespace Hot4.ViewModel
{
    public class RechargePrepaidModel
    {
        public long RechargeId { get; set; }
        public bool DebitCredit { get; set; }
        public string ReturnCode { get; set; }
        public string Narrative { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public string Reference { get; set; }
        public decimal? InitialWallet { get; set; }
        public decimal? FinalWallet { get; set; }
        public DateTime? Window { get; set; }
        public string Data { get; set; }
        public string SMS { get; set; }
    }
}
