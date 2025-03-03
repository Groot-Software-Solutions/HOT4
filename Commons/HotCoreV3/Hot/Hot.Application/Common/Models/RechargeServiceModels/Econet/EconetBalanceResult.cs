namespace Hot.Application.Common.Models.RechargeServiceModels.Econet;

public class EconetBalanceResult
{
    public bool Successful { get; set; }
    public string? RawResponseData { get; set; }
    public string? TransactionResult { get; set; }
    public decimal Balance { get; set; }
    public string ResponseCode { get; set; } = string.Empty;
}



