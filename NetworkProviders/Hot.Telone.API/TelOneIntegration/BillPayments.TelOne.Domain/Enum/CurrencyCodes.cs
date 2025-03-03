using System;
using System.Collections.Generic;
using System.Text;

namespace TelOne.Domain.Enum
{
    public class CurrencyCodes
    {
        public string Value { get; set; }
        private CurrencyCodes(string value) { Value = value; }
        public static implicit operator string(CurrencyCodes item) => item.Value;

        public static CurrencyCodes ZWG => new CurrencyCodes("ZWG");
        public static CurrencyCodes ZiG => new CurrencyCodes("ZiG");
        public static CurrencyCodes RTGS => new CurrencyCodes("RTGS");
        public static CurrencyCodes USD => new CurrencyCodes("USD");
    }
}
