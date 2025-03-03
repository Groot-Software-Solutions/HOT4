using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Transfer
    {
        public long TransferId { get; set; }
        public byte ChannelId { get; set; }
        public long PaymentId_From { get; set; }
        public long PaymentId_To { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public long SmsId { get; set; }
    }
}
