using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class HotType
    {
        public int HotTypeID { get; set; }
        public string HotTypeName { get; set; } = string.Empty;
        public short SplitCount { get; set; }
    }
}
