using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Domain.Models
{
    public class BroadbandProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public bool Activated { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
    }


}
