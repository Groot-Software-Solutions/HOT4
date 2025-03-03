namespace Hot.Application.Common.Models.RechargeServiceModels.Telecel
{
    public class TelecelRechargeResult: RechargeServiceResult
    { 
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; } 
        public string Narrative { get; set; }
        public string ReturnCode { get; set; }
    }
}
