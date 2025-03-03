using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class BulkEvdResponse
    {
        public int ReplyCode{get; set;} = 0;
        public string ReplyMsg{get; set;} = "";
        public string ReplyMessage { 
            get { return ReplyMsg; } 
            set { ReplyMsg = value; }
        }
        public decimal WalletBalance{get; set;} = 0 ;
        public decimal Amount{get; set;} = 0;
        public decimal Discount{get; set;} = 0;
        public List<string> Pins{get; set;} 
        public string AgentReference{get; set;} = "";
        public int RechargeID{get; set;} = 0;
    }
}
