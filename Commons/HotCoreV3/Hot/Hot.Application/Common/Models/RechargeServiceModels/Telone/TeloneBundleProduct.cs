namespace Hot.Application.Common.Models.RechargeServiceModels.Telone;

public class TeloneBundleProduct
{ 
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public int ProductId { get; set; }
    public Brands BrandId { get; set; }
}
