using System;

namespace Sage.Domain.Entities
{
    public class CustomerSalesHistory
    {
        public Customer Customer { get; set; } = new Customer();
        public DateTime Date { get; set; }
        public decimal Exclusive { get; set; }
    }


}
