using System;
using System.Collections.Generic;

namespace Sage.Domain.Entities
{
    public class CustomerStatement
    {
        public Customer Customer { get; set; } = new Customer();
        public List<CustomerStatementLine> StatementLines { get; set; } = new List<CustomerStatementLine>();
        public DateTime Date { get; set; }
        public decimal TotalDue { get; set; } = 0;
        public decimal TotalPaid { get; set; } = 0;
        public decimal Current { get; set; } = 0;
        public decimal Days30 { get; set; } = 0;
        public decimal Days60 { get; set; } = 0;
        public decimal Days90 { get; set; } = 0;
        public decimal Days120Plus { get; set; } = 0;
        public string Message { get; set; } = "";
    }


}
