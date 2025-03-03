namespace Hot.Application.Common.Models.RechargeServiceModels.ZESA;

public class PurchaseTokenModel
{
    public string Reference { get; set; }

    public System.DateTime Date { get; set; }

    public double Amount { get; set; }

    public string MeterNumber { get; set; }

    public string RawResponse { get; set; }

    public string ResponseCode { get; set; }

    public string Narrative { get; set; }

    public string VendorReference { get; set; }

    public List<TokenItemModel> Tokens { get; set; }

}

