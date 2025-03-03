namespace Hot.Application.Common.Interfaces;
public interface IRechargeHandler
{
    public int BrandId { get; set; }
    public RechargeType Rechargetype { get; set; }
    public Task<OneOf<RechargeResult, RechargeServiceNotFoundException>> ProcessAsync(Recharge recharge, RechargePrepaid rechargePrepaid);

}

