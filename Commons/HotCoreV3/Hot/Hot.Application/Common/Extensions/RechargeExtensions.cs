namespace Hot.Application.Common.Extensions;

public static class RechargeExtensions
{
    public static string GetStatus(this Recharge recharge, List<State> list)
    {
        var result = list.Where(i => i.StateID == recharge.StateId).FirstOrDefault();
        if (result == null)
            return "Unknown";
        return result.Name;
    }
    public static string GetBrand(this Recharge recharge, List<Brand> list)
    {
        var result = list.Where(i => i.BrandId == recharge.BrandId).FirstOrDefault();
        if (result == null)
            return "Unknown";
        return result.BrandName;
    }

    public static decimal GetRechargeCost(this decimal amount, decimal discount)
    {
        return amount * ((100M - discount) / 100M);
    }
    public static decimal GetRechargeCost(this double amount, double discount)
    {
        return GetRechargeCost((decimal)amount, (decimal)discount);
    }

    public static string ToTargetMobileWithSuffix(this string TargetMobile, Currency currency) => $"{TargetMobile}{(currency == Currency.USD ? "U" : "")}";
}