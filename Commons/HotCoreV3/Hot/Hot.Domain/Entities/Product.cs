using Hot.Domain.Enums;

namespace Hot.Domain.Entities;

public class Product 
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int WalletTypeId { get; set; }
    public int BrandId { get; set; }
    public ProductStates ProductStateId { get; set; }
}
