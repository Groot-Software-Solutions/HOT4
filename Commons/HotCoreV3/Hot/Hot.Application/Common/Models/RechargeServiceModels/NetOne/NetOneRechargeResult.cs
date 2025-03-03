namespace Hot.Application.Common.Models.RechargeServiceModels.NetOne
{
    public class NetOneRechargeResult : RechargeServiceResult
    { 
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; } 
        public string Narrative { get; set; }
        public string ReturnCode { get; set; }
        public decimal? RechargeId { get; set; }
    }
}

