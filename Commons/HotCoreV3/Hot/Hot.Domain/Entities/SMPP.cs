using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class SMPP
    {
        public int SmppID { get; set; }
        public string SmppName { get; set; } = string.Empty;
        public bool AllowSend { get; set; }
        public bool AllowReceive { get; set; }
        public bool SmppEnabled { get; set; }
        public int DestinationAddressNpi { get; set; }
        public int DestinationAddressTon { get; set; }
        public string SourceAddress { get; set; } = String.Empty;
        public int SourceAddressNpi { get; set; }
        public int SourceAddressTon { get; set; }
        public int SmppTimeout { get; set; }
        public string RemoteHost { get; set; } = string.Empty;
        public int RemotePort { get; set; }
        public string SystemID { get; set; } = string.Empty;
        public string SmppPassword { get; set; } = string.Empty ;
        public string? AddressRange { get; set; } 
        public int InterfaceVersion { get; set; }
        public string SystemType { get; set; } = string.Empty;
        public string EconetPrefix { get; set; } = string.Empty;
        public string NetOnePrefix { get; set; } = string.Empty;
        public string TelecelPrefix { get; set; } = string.Empty;
    }
}
