using System;
using System.Collections.Generic;

namespace Hot4.DataModel.Models;

public partial class ZtblArchive
{
    public DateTime ArchiveRunDate { get; set; }

    public DateTime ArchiveEffectiveDate { get; set; }

    public long MaxRechargeId { get; set; }

    public long MaxSmsid { get; set; }

    public long MaxPaymentId { get; set; }
}
