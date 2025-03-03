using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class PinBatch
    {
        public int PinBatchId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime BatchDate { get; set; }
        public byte PinBatchTypeId { get; set; }
    }
}
