using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities;

public class Brand
{
    public byte BrandId { get; set; }
    public byte NetworkId { get; set; }
    public string BrandName { get; set; } = string.Empty;
    public string BrandSuffix { get; set; } = string.Empty;
    public int WalletTypeId { get; set; }

}
