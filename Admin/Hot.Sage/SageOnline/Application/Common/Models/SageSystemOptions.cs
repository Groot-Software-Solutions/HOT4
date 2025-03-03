using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Application.Common.Models
{
    public static class SageSystemOptions
    {
        public static int HotCustomerId { get; set; }
        public static int HotAirtimeItemId { get; set; }
        public static int HotSalesRepId { get; set; }
        public static int HotDefaultTaxId { get; set; }
        public static int HotZesaTaxId { get; set; }
        public static Dictionary<int, int> HotProfileCategory { get; set; }
        public static int HotZesaItemId { get; set; }
        public static int HotUtilityItemId { get; set; }
        public static int HotUSDTaxId { get; set; }
    }
}
