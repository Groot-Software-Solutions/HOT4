namespace Hot.Application.Common.Interfaces;
public interface IRechargeHandlerFactory
{
    public bool HasService(int BrandId);
    public IRechargeHandler GetService(int BrandId);
    public RechargeType GetRechargeType(int BrandId);
}
