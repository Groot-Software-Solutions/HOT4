using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Enums
{
    public class Merchants
    { 
        public string Value { get; set; }
        private Merchants(string value) { Value = value; }
        public static implicit operator string(Merchants item) => item.Value;

        public static Merchants ZETDC => new Merchants("ZETDC");
        public static Merchants NETONE => new Merchants("NETONE");
        public static Merchants TELECEL => new Merchants("TELECEL");
        public static Merchants EDGARS => new Merchants("EDGARS");
        public static Merchants DSTV => new Merchants("DSTV");
        public static Merchants ECONET => new Merchants("ECONET");
        public static Merchants TELONE => new Merchants("TELONE");
        public static Merchants ZOL => new Merchants("ZOL");
        public static Merchants BYOBILL => new Merchants("BYOBILL");
        public static Merchants HARARE => new Merchants("HARARE");
        public static Merchants GWERU => new Merchants("GWERU");
    }
}
