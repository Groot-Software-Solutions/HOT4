using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Enums
{
    public class Products
    {

        public string Value { get; set; }
        private Products(string value) { Value = value; }
        public static implicit operator string(Products item) => item.Value;

        public static Products ZETDC_PREPAID => new Products("ZETDC_PREPAID");
        public static Products NETONE_AIRTIME => new Products("NETONE_AIRTIME");
        public static Products TELECEL_AIRTIME => new Products("TELECEL_AIRTIME");
        public static Products ECONET => new Products("ECONET_AIRTIME");
        public static Products EDGARS => new Products("EDGARS");
        public static Products DSTV => new Products("DSTV");
        public static Products ZOL => new Products("ZOL");
        public static Products ZOL_DATA => new Products("ZOL_DATA");
        public static Products BYOBILL => new Products("BYOBILL");
        public static Products HARARE => new Products("HARARE");
        public static Products GWERU => new Products("GWERU");
        public static Products TELONE => new Products("HOME_PLUS");
    }
}
