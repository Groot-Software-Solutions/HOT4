using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class SMS
    {
        public long SMSID { get; set; }
        public int? SmppID { get; set; }
        public State? State { get; set; } 
        public Priority? Priority { get; set; }
        public bool Direction { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string SMSText { get; set; } = string.Empty ;
        public DateTime SMSDate { get; set; }
        public long? SMSID_In { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
