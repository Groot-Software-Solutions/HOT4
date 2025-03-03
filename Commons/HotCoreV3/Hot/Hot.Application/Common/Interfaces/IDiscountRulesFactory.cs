namespace Hot.Application.Common.Interfaces;
public interface IDiscountRulesFactory
{
    List<IDiscountRule> GetRules();
}