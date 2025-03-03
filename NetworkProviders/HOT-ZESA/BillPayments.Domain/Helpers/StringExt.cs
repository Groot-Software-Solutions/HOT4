using System;
using System.Collections.Generic;
using System.Text;

namespace BillPayments.Domain.Helpers
{
    public static class StringExt
    {
        public static bool IsNumeric(this string text) => double.TryParse(text, out _);

    }
}
