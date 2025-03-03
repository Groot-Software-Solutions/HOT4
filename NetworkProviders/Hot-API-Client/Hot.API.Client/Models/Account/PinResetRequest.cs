using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.API.Client.Models
{
    public class PinResetRequest
    {
        public string RequestingNumber { get; set; }
        public string TargetNumber { get; set; }
        public string IDNumber { get; set; }
        public string Names { get; set; }

    }

}
