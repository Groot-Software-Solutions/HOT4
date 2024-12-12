﻿using System;
using System.Collections.Generic;

namespace Hot4.DataModel.Models;

public partial class TblStockData
{
    public string? BrandName { get; set; }

    public decimal? PinValue { get; set; }

    public int? Available { get; set; }

    public DateTime? LastSold { get; set; }

    public decimal? Sold { get; set; }

    public decimal? WeekSold { get; set; }

    public decimal? MonthSold { get; set; }
}
