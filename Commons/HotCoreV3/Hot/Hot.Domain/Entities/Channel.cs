using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Channel
    {  
        public byte ChannelId { get; set; }
        public string ChannelText { get; set; } = string.Empty;
    }
}
