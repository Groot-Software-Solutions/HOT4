using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Entities
{
    public class Item
    {
        public int ID { get; set; } = 0;
        public string Description { get; set; } = "";
        public ItemCategory Category { get; set; } = new ItemCategory();
        public string Code { get; set; } = "";
        public bool Active { get; set; } = true;
        public decimal PriceInclusive { get; set; } = 0;
        public decimal PriceExclusive { get; set; } = 0;
        public bool Physical { get; set; } = false;
        public int TaxTypeIdSales { get; set; } = 0;
        public int TaxTypeIdPurchases { get; set; } = 0;
        public decimal LastCost { get; set; } = 0;
        public decimal AverageCost { get; set; } = 0;
        public decimal QuantityOnHand { get; set; } = 0;
        public decimal TotalQuantity { get; set; } = 0;
        public decimal TotalCost { get; set; } = 0;
         
    }


}
