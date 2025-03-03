namespace Hot.Application.Common.Models.RechargeServiceModels.ZESA;
public class ZESAPurchaseTokenResult: RechargeServiceResult
{  
    public int ReturnCode { get; set; } 
    public PurchaseTokenModel PurchaseToken { get; set; }
    public CustomerInfoModel CustomerInfo { get; set; }
}

