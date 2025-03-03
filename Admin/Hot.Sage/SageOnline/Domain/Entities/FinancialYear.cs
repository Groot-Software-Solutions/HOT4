using System;

namespace Sage.Domain.Entities
{
    public class FinancialYear
    {
        public int ID { get; set; } = 0;
        public DateTime YearStart { get; set; }
        public DateTime YearEnd { get; set; }
        public bool IsCurrentYear { get; set; } = false;
    }


}
