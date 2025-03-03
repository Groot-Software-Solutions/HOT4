using Sage.Domain.Enums;

namespace Sage.Domain.Entities
{
    public class Company
    {
        public int ID { get; set; }
        public string Name { get; set; } = ""; // 100 Max
        public int CurrencyDecimalDigits { get; set; } = 0; // 0-4
        public int NumberDecimalDigits { get; set; } = 0; // 0-4
        public string DecimalSeparator { get; set; } = ".";
        public string GroupSeparator { get; set; } = ",";
        public int RoundingValue { get; set; } = 0;
        public TaxSystem TaxSystem { get; set; } = TaxSystem.InvoiceBased;
        public RoundingType RoundingType { get; set; } = RoundingType.Normal;
    }




}
