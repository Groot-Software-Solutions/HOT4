using System;

namespace Hot.Domain.Entities
{
    public class RechargePrepaid
    {
        public long RechargeId { get; set; }
        public bool DebitCredit { get; set; }
        public string ReturnCode { get; set; } = string.Empty;  
        public string Narrative { get; set; } = string.Empty;
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public string Reference { get; set; } = string.Empty;   
        public decimal? InitialWallet { get; set; }
        public decimal? FinalWallet { get; set; }
        public DateTime? Window { get; set; }
        public string? Data { get; set; }
        public string? SMS { get; set; } = string.Empty;
    }
}
