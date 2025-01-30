using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Core.Settings
{

    public class NetworkSettings
    {
        public ZesaSettings? Zesa {  get; set; }
        public TeloneSettings? Telone { get; set; }
    }
    public class ZesaSettings
    {
        public  string? HotAPIKey { get; set; }
        public int TimeoutPeriod { get; set; }
    }

    public class TeloneSettings
    {
        public string? HotAPIKey { get; set; }
        public int TimeoutPeriod { get; set; }
    }
}
