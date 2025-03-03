using System;

namespace Sage.Domain.Entities
{
    public class RecurringInvoice
    {
        public int DocumentHeaderId { get; set; } = 0;
        public int Period { get; set; } = 0;
        public string PeriodDisplayText { get; set; } = "";
        public string CustomerName { get; set; } = "";
        public string DocumentNumber { get; set; } = "";
        public DateTime Date { get; set; }
        public bool Printed { get; set; } = false;
        public decimal Total { get; set; } = 0;
        public string BaseUrl { get; set; } = "";
    }


}
