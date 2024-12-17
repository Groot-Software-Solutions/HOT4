using System;
using System.Collections.Generic;

namespace Hot4.DataModel.Models;

public partial class WebRequest
{
    public long WebId { get; set; }

    public byte? HotTypeId { get; set; }

    public long? AccessId { get; set; }

    public byte StateId { get; set; }

    public DateTime InsertDate { get; set; }

    public DateTime? ReplyDate { get; set; }

    public string? AgentReference { get; set; }

    public int? ReturnCode { get; set; }

    public string? Reply { get; set; }

    public byte ChannelId { get; set; }

    public long? RechargeId { get; set; }

    public decimal? WalletBalance { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Discount { get; set; }

    public decimal? Amount { get; set; }
}
