using System;
using System.Collections.Generic;

namespace Sage.Domain.Entities
{
    public class IncomeVSExpense
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<IncomeVSExpensesPeriodGrouping> Periods { get; set; } = new List<IncomeVSExpensesPeriodGrouping>();
        public bool IsComparative { get; set; }
        public bool UsePurchases { get; set; }
    }


}
