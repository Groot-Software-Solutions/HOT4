namespace Hot.Application.Common.Models.RechargeServiceModels.ZESA;

public class ZESAAccountQueryResult
{ 
    public bool Successful { get; set; }
    public string? TransactionResult { get; set; }
    public CustomerInfoModel CustomerInfo { get; set; }
}

