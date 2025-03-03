using System;

namespace Sage.Domain.Entities
{
    public class TaxType
    {
        public int ID { get; set; } = 0;
        public string Name { get; set; } = "";
        public decimal Percentage { get; set; } = 0;
        public bool IsDefault { get; set; } = false;
        public bool HasActivity { get; set; } = false;
        public bool IsManualTax { get; set; } = false;
        public DateTime Modified { get; set; }
        public DateTime Created { get; set; }

    }


}
