using System;
using System.Collections.Generic;
using System.Text;

namespace Hot.Domain.Entities
{
    public class Address
    {

        public long AccountID { get; set; }

        public string Address1 { get; set; } = string.Empty;

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string ContactName { get; set; } = string.Empty;

        public string ContactNumber { get; set; } = string.Empty;

        public string? VatNumber { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public long? SageID { get; set; }
        public long? SageIDUsd { get; set; }

        public byte? InvoiceFreq { get; set; }

        public bool Confirmed { get; set; }
    }
}
