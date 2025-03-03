using Sage.Application.Common.Models;
using Sage.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.Application.Common.Helpers
{
    public static class HotSageHelper
    { 
        public static int GetBankFromSource(int paysource)
        { 
            try
            {
                return (int)Enum.Parse(typeof(HotBanks), Enum.GetName(typeof(HotBanksPaymentSource), paysource));
            }
            catch (Exception)
            {
                return (int)HotBanks.Office_Cash;
            }
        }
        public static int GetCategoryFromProfileID(int ProfileID)
        {
            if (SageSystemOptions.HotProfileCategory.ContainsKey(ProfileID) == true)
                return SageSystemOptions.HotProfileCategory[ProfileID];
            else
                return 207582;
        }
    }
}
