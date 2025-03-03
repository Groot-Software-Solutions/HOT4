using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Models
{
    public class BulkEvdRequest 
    {
        public int BrandId { get; set; }
        public decimal Denomination { get; set; }
        public int Quantity { get; set; }
    }
}
