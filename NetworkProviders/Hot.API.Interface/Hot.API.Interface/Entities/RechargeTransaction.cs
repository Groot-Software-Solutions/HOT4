using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.API.Interface.Entities
{
    public class RechargeTransaction
    {
        public int RechargeID { get; set; }
        public string Mobile { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; } 
        public decimal InitialBalance { get; set; }
        public decimal FinalBalance { get; set; }
        public DateTime RechargeDate { get; set; }
        public string Narrative { get; set; }
        public string State { get; set; }
        public string BrandName { get; set; }
        public string AccessCode { get; set; }

        public decimal Cost { get { return ((Amount * (100 - Discount)) / 100); }}
    }
}
