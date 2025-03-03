using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Network
    {

        public byte NetworkId { get; set; }
        public string NetworkName { get; set; } = string.Empty;
        public string Prefix { get; set; } = string.Empty;
        public string EndPointAddress { get; set; } = string.Empty;
        public string? EndPointAddressTest { get; set; } 
    }
}
