using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Reply
    {
        public Template? Template { get; set; }
        public string Mobile { get; set; } = string.Empty;
    }
}
