
namespace Hot.Infrastructure.FactoryServices;

public class RechargeDataQueryHandlerFactory : IRechargeDataQueryHandlerFactory
{
    private readonly IReadOnlyDictionary<int, IRechargeDataQueryHandler> Handlers;

    public RechargeDataQueryHandlerFactory(IServiceProvider serviceProvider)
    {
        var serviceType = typeof(IRechargeDataQueryHandler);
        var AssemblyMarker = typeof(RechargeHandlerFactory);

        Handlers = AssemblyMarker.Assembly.ExportedTypes
             .Where(x => serviceType.IsAssignableFrom(x)
                && !x.IsInterface && !x.IsAbstract)
             .Select(y => { return Activator.CreateInstance(y, serviceProvider); })
             .Cast<IRechargeDataQueryHandler>()
             .ToDictionary(item => item.NetworkId, item => item);
    }
    public List<IRechargeDataQueryHandler> GetServices() 
    {
        return Handlers.Select(s => s.Value).ToList();
    }
    public bool HasService(int BrandId)
    {
        return Handlers.ContainsKey(BrandId);
    }

    public IRechargeDataQueryHandler GetService(int BrandId)
    {
        if (HasService(BrandId)) return Handlers[BrandId];
        throw new KeyNotFoundException("Service Does not Exist");
    }
}
