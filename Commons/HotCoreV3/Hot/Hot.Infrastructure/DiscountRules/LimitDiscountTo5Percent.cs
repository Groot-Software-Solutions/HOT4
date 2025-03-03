using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities; 

namespace Hot.Infrastructure.DiscountRules
{
    public class LimitDiscountTo5Percent : IDiscountRule
    {
        public void Apply(Recharge recharge)
        {
            if (recharge.Amount < 1 && recharge.Discount > 5) recharge.Discount = 5;
        }
    }
}
