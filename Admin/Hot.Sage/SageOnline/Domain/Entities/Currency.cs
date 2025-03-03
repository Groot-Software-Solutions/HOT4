using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Domain.Entities
{
    public class Currency
    { 
        public int ID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; } 
    }
}
