using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Models
{
    public class RechargeResponse : Response
    {
        public string WalletBalance;
        public decimal Amount;
        public decimal InitialBalance;
        public decimal FinalBalance;
        public new string ReplyMsg
        {
            get
            {
                return (_ReplyMsg ?? ReplyMessage);
            }
            set
            {
                _ReplyMsg = value;
            }
        }
        private string _ReplyMsg;
        public string ReplyMessage;
        public DateTime? Window;
        public string Data;
        public string SMS;
        public int RechargeID;


    }

    public class RechargeBundledResponse : RechargeResponse
    {
        public List<RechargeResponse> Recharges { get; set; } = new List<RechargeResponse>();
    }
}
