using Hot.Application.Common.Interfaces;
using Hot.Domain.Enums;

namespace Hot.Infrastructure.FactoryServices;
public class RechargeHandlerFactory : IRechargeHandlerFactory
{
    private readonly IReadOnlyDictionary<int, IRechargeHandler> RechargeServices;

    public RechargeHandlerFactory(IDbContext dbContext, IServiceProvider serviceProvider)
    {
        var serviceType = typeof(IRechargeHandler);
        var AssemblyMarker = typeof(RechargeHandlerFactory);

        RechargeServices = AssemblyMarker.Assembly.ExportedTypes
             .Where(x => serviceType.IsAssignableFrom(x)
                && !x.IsInterface && !x.IsAbstract)
             .Select(y => { return Activator.CreateInstance(y, dbContext, serviceProvider); })
             .Cast<IRechargeHandler>()
             .ToDictionary(item => item.BrandId, item => item);
    }

    public bool HasService(int BrandId)
    {
        return RechargeServices.ContainsKey(BrandId);
    }

    public IRechargeHandler GetService(int BrandId)
    {
        if (HasService(BrandId)) return RechargeServices[BrandId]; 
        throw new KeyNotFoundException("Service Does not Exist");
    }

    public RechargeType GetRechargeType(int BrandId)
    {
        if (HasService(BrandId)) return RechargeServices[BrandId].Rechargetype;
        throw new KeyNotFoundException("Service Does not Exist");
    }

}
