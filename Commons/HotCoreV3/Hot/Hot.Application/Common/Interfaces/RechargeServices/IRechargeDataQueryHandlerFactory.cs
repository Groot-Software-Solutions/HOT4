namespace Hot.Application.Common.Interfaces;

public interface IRechargeDataQueryHandlerFactory
{
    public bool HasService(int BrandId);
    public IRechargeDataQueryHandler GetService(int BrandId);
    public List<IRechargeDataQueryHandler> GetServices();
}


