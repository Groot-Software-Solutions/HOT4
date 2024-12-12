﻿namespace Hot4.DataModel.Models;

public partial class VwzSm
{
    public long Smsid { get; set; }

    public byte? SmppId { get; set; }

    public byte StateId { get; set; }

    public required string State { get; set; }

    public byte PriorityId { get; set; }

    public required string Priority { get; set; }

    public bool Direction { get; set; }

    public required string Mobile { get; set; }

    public required string Smstext { get; set; }

    public DateTime Smsdate { get; set; }

    public DateTime? Insertdate { get; set; }

    public long? SmsidIn { get; set; }
}
