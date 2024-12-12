using System;
using System.Collections.Generic;

namespace Hot4.DataModel.Models;

public partial class VwTempRechargeDb
{
    public long RechargeId { get; set; }

    public byte StateId { get; set; }

    public long NRecharge { get; set; }

    public byte NStateId { get; set; }
}
