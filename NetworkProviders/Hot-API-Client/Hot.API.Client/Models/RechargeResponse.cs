using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Models
{
    public class RechargeResponse : Response
    {
        public decimal WalletBalance { get; set; }
        public decimal Amount { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public new string ReplyMsg
        {
            get
            {
                return (_replyMsg ?? ReplyMessage);
            }
            set
            {
                _replyMsg = value;
            }
        }
        private string _replyMsg = "";
        public new string ReplyMessage = "";
        //public DateTime? Window { get; set; }
        public string Data { get; set; } = "";
        public string SMS { get; set; } = "";
        public int RechargeID { get; set; }
        public decimal Discount { get; set; }

    }
}
