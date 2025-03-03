using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime;

public class EconetLesothoRechargeHandler : EconetRechargeHandler, IRechargeHandler
{
    public EconetLesothoRechargeHandler(IDbContext dbcontext, IServiceProvider serviceProvider) : base(dbcontext, serviceProvider)
    {
    }
    public new int BrandId { get; set; } = (int)Brands.EconetLesotho; 
    internal new virtual string GetReference()
    {
        return $"HOTRECHARGE{Recharge.Mobile.ToMobile()}{DateTime.Now:yyyyMMddHHmmssfff}";
    }
}


