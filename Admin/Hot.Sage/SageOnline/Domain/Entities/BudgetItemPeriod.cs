using System;

namespace Sage.Domain.Entities
{
    public class BudgetItemPeriod
    {
        public int ID { get; set; } = 0;
        public DateTime Date { get; set; }
        public decimal Total { get; set; } = 0;
    }




}
