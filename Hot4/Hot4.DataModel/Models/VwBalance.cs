using System;
using System.Collections.Generic;

namespace Hot4.DataModel.Models;

public partial class VwBalance
{
    public long AccountId { get; set; }

    public decimal? Balance { get; set; }

    public decimal? Zesabalance { get; set; }

    public decimal? Usdbalance { get; set; }

    public decimal? UsdutilityBalance { get; set; }

    public decimal? SaleValue { get; set; }
}
