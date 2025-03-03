namespace Hot.Application.Common.Models.RechargeServiceModels.Econet;

public class EconetDataRechargeResult: RechargeServiceResult
{ 
    public string ProductCode { get; set; } = string.Empty; 
    public string Reference { get; set; } = string.Empty;  
    public string StatusCode { get; set; } = string.Empty;

}
