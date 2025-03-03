namespace Hot.Application.Common.Models.RechargeServiceModels.Econet;

public class EconetRechargeResult: RechargeServiceResult
{
    public decimal InitialBalance { get; set; }
    public decimal FinalBalance { get; set; }
    public string ResponseCode { get; set; } = string.Empty;
}
