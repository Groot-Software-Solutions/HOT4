using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class Response
    {
        public int ReplyCode { get; set; }
        public string ReplyMsg { get; set; } = "";
        public string AgentReference { get; set; } = "";

        public string ReplyMessage { set {  ReplyMsg = value; } }

    }
}
