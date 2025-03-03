using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Client.Entities
{
    public class Network
    {
        public enum Networks
        {
            Econet = 1,
            NetOne = 2,
            Telecel = 3,
            Africom = 4,
            Umax = 5,
            Powertel = 6,
            Econet078 = 7,
        }

        public byte NetworkId { get; set; }
        public string NetworkName { get; set; }
        public string Prefix { get; set; }
        public string EndPointAddress { get; set; }
        public string EndPointAddressTest { get; set; }
    }
}
