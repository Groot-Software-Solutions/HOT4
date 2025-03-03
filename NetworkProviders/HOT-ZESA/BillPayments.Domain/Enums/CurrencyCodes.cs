using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Enums
{
    public class CurrencyCodes
    {
        public string Value { get; set; }
        private CurrencyCodes(string value) { Value = value; }
        public static implicit operator string(CurrencyCodes item) => item.Value;

        public static CurrencyCodes ZWG => new CurrencyCodes("ZWG");
        public static CurrencyCodes USD => new CurrencyCodes("USD");
    }
}
