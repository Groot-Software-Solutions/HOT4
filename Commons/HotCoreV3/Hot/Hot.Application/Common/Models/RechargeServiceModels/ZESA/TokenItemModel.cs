namespace Hot.Application.Common.Models.RechargeServiceModels.ZESA;

public class TokenItemModel
{
    public string ZesaReference { get; set; } 
    public string Token { get; set; }
    public double Units { get; set; }

    public double NetAmount { get; set; }

    public double Arrears { get; set; }

    public double Levy { get; set; }

    public double TaxAmount { get; set; }

}

