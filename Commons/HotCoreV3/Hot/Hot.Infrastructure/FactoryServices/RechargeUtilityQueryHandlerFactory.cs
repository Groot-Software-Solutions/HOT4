namespace Hot.Infrastructure.FactoryServices;

public class RechargeUtilityQueryHandlerFactory : IRechargeUtilityQueryHandlerFactory
{
    private readonly IReadOnlyDictionary<int, IRechargeUtilityQueryHandler> Handlers;

    public RechargeUtilityQueryHandlerFactory(IServiceProvider serviceProvider)
    {
        var serviceType = typeof(IRechargeUtilityQueryHandler);
        var AssemblyMarker = typeof(RechargeDataQueryHandlerFactory);

        Handlers = AssemblyMarker.Assembly.ExportedTypes
             .Where(x => serviceType.IsAssignableFrom(x)
                && !x.IsInterface && !x.IsAbstract)
             .Select(y => { return Activator.CreateInstance(y, serviceProvider); })
             .Cast<IRechargeUtilityQueryHandler>()
             .ToDictionary(item => item.NetworkId, item => item);
    }

    public bool HasService(int BrandId)
    {
        return Handlers.ContainsKey(BrandId);
    }

    public IRechargeUtilityQueryHandler GetService(int BrandId)
    {
        if (HasService(BrandId)) return Handlers[BrandId];
        throw new KeyNotFoundException("Service Does not Exist");
    }
}
