using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Pin
    {
        public long PinId { get; set; }
        public long PinBatchId { get; set; }
        public byte PinStateId { get; set; }
        public byte BrandId { get; set; }
        public string PinNumber { get; set; } = string.Empty;
        public string PinRef { get; set; } = String.Empty;
        public decimal PinValue { get; set; }
        public DateTime PinExpiry { get; set; }
    }
}
