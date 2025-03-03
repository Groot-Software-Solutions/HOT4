namespace Hot.Application.Common.Interfaces;

public interface IRechargeUtilityQueryHandlerFactory
{
    public bool HasService(int BrandId);
    public IRechargeUtilityQueryHandler GetService(int BrandId);
}
