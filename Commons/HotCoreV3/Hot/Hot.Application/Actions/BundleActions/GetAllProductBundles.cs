namespace Hot.Application.Actions.BundleActions;

public record GetAllProductBundles : IRequest<OneOf<List<BundleModel>, AppException>>;

public class GetAllProductBudlesHandler : IRequestHandler<GetAllProductBundles, OneOf<List<BundleModel>, AppException>>
{
    public readonly IRechargeDataQueryHandlerFactory handlerFactory;
    public readonly ILogger<GetAllProductBudlesHandler> logger;

    public GetAllProductBudlesHandler(IRechargeDataQueryHandlerFactory handlerFactory, ILogger<GetAllProductBudlesHandler> logger)
    {
        this.handlerFactory = handlerFactory;
        this.logger = logger;
    }

    public async Task<OneOf<List<BundleModel>, AppException>> Handle(GetAllProductBundles request, CancellationToken cancellationToken)
    {
        try
        {
            List<BundleModel> list = new();
            foreach (var service in handlerFactory.GetServices())
            {
                var response = await service.GetBundles();
                if (response.IsT0)
                    list.AddRange(response.AsT0);
            }
            return list
                    .GroupBy(b => b.BundleID)
                    .Select(g => g.First())
                    .ToList();
        }
        catch (Exception ex)
        {
            return ex.LogAndReturnError(logger);
        }
    }
}