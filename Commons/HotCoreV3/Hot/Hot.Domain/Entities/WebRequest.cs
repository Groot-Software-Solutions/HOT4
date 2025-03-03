using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Domain.Entities;
public class WebRequest
{
    public long WebID { get; set; }
    public byte HotTypeID { get; set; }
    public long AccessID { get; set; }
    public byte StateID { get; set; }
    public DateTime InsertDate { get; set; }
    public DateTime ReplyDate { get; set; }
    public string AgentReference { get; set; } = string.Empty;
    public int ReturnCode { get; set; }
    public string? Reply { get; set; }  
    public byte ChannelID { get; set; }
    public long? RechargeID { get; set; }
    public decimal? WalletBalance { get; set; }
    public decimal? Cost { get; set; }
    public decimal? Discount { get; set; }
    public decimal Amount { get; set; }
    public bool IsRequest { get; set; }

}

public class RawRequest
{
    public long RawRequestID { get; set; }
    public string AgentReference { get; set; }
    public string AccessCode { get; set; }
    public string Headers { get; set; }
    public string Method { get; set; }
    public string AbsoluteUri { get; set; }
    public string Body { get; set; }
    public string StatusCode { get; set; }
    public string ResponseBody { get; set; }
    public DateTime RequestDate { get; set; }
    public DateTime? ResponseDate { get; set; }
}
