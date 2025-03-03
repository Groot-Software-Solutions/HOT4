using Hot.Application.Common.Interfaces;

namespace Hot.Infrastructure.FactoryServices
{
    public class DiscountRulesFactory : IDiscountRulesFactory
    {
        private readonly List<IDiscountRule> DiscountRules;

        public DiscountRulesFactory()
        {
            var discountRule = typeof(IDiscountRule);

            var AssemblyMarker = typeof(RechargeHandlerFactory);

            DiscountRules = AssemblyMarker.Assembly.ExportedTypes
                 .Where(x => discountRule.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .Select(y => { return Activator.CreateInstance(y); })
                 .Cast<IDiscountRule>()
                 .ToList();
        }

        public List<IDiscountRule> GetRules()
        {
            return DiscountRules.Any() ? DiscountRules : new();
        }
    }
}
