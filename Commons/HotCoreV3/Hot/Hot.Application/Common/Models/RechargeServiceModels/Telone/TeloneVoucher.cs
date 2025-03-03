namespace Hot.Application.Common.Models.RechargeServiceModels.Telone;

public  class TeloneVoucher
{ 
    public int ProductId { get; set; }

    public string Pin { get; set; } = string.Empty;
     
    public string BatchNumber { get; set; } = string.Empty;

    public string SerialNumber { get; set; } = string.Empty;

    public string CardNumber { get; set; } = string.Empty;
}
