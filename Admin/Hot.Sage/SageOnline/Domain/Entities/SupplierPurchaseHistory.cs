using System;

namespace Sage.Domain.Entities
{
    public class SupplierPurchaseHistory
    {
        public Supplier Supplier { get; set; } = new Supplier();
        public DateTime Date { get; set; }
        public decimal Exclusive { get; set; }
    }


}
