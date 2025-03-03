using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class AccessWeb
    {
        public long AccessID { get; set; }

        public string? AccessName { get; set; }

        public string? WebBackground { get; set; }

        public bool? SalesPassword { get; set; }

        public string? ResetToken { get; set; }

    }
}
