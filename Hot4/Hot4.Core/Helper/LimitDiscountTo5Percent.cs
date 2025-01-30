using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Core.Helper
{
   public class LimitDiscountTo5Percent
    {
        public void Apply(RechargeModel rechargeModel)
        {
            if (rechargeModel.Amount < 1 && rechargeModel.Discount > 5)
            {
                rechargeModel.Discount = 5;
            }
        }
    }
}
