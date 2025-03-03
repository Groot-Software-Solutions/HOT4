namespace Hot.Application.Common.Models.RechargeServiceModels
{
    public class RechargeServiceResult
    { 
        public string? TransactionResult { get; set; }
        public bool Successful { get; set; }
        public string? RawResponseData { get; set; } 
        public decimal InitialWallet { get; set; } = 0;
        public decimal FinalWallet { get; set; } = 0;
    }
}
