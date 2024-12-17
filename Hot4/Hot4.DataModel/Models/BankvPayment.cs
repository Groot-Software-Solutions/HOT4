using System;
using System.Collections.Generic;

namespace Hot4.DataModel.Models;

public partial class BankvPayment
{
    public long? BankTrxId { get; set; }

    public Guid? VPaymentId { get; set; }

    public string? CheckUrl { get; set; }

    public string? ProcessUrl { get; set; }

    public string? ErrorMsg { get; set; }

    public string? VPaymentRef { get; set; }
}
