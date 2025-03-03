
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Domain.Entities;

public class TelecelReconZWL
{
    
    public int Code { get; set; } 
     
    public string DestMobile { get; set; } = string.Empty;
     
    public string AgentId { get; set; } = string.Empty;
     
    public string AirwideTid { get; set; } = string.Empty;
     
    public decimal Amount { get; set; } 
     
    public decimal  AgentOpeningBalance { get; set; } 
     
    public decimal  AgentClosingBalance { get; set; }
     
    public string ResultDescription { get; set; } = string.Empty;
     
    public DateTime RequestTime { get; set; } 
     
    public DateTime ResponseTime { get; set; } 
     
    public string ResponseCode { get; set; } = string.Empty;

}
