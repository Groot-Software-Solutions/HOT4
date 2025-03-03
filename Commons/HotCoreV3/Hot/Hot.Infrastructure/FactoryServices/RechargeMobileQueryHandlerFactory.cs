namespace Hot.Infrastructure.FactoryServices;

public class RechargeMobileQueryHandlerFactory : IRechargeMobileQueryHandlerFactory
{
    private readonly IReadOnlyDictionary<int, IRechargeMobileQueryHandler> Handlers;

    public RechargeMobileQueryHandlerFactory( IServiceProvider serviceProvider)
    {
        var serviceType = typeof(IRechargeMobileQueryHandler);
        var AssemblyMarker = typeof(RechargeDataQueryHandlerFactory);

        Handlers = AssemblyMarker.Assembly.ExportedTypes
             .Where(x => serviceType.IsAssignableFrom(x)
                && !x.IsInterface && !x.IsAbstract)
             .Select(y => { return Activator.CreateInstance(y, serviceProvider); })
             .Cast<IRechargeMobileQueryHandler>()
             .ToDictionary(item => item.NetworkId, item => item);
    }

    public bool HasService(int BrandId)
    {
        return Handlers.ContainsKey(BrandId);
    }

    public IRechargeMobileQueryHandler GetService(int BrandId)
    {
        if (HasService(BrandId)) return Handlers[BrandId];
        throw new KeyNotFoundException("Service Does not Exist");
    }
}
