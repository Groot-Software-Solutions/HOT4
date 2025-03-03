namespace Hot.Application.Actions.BundleActions;
public record GetAllBundles : IRequest<OneOf<List<Bundle>, AppException>>;

public class GetAllBudlesHandler : IRequestHandler<GetAllBundles, OneOf<List<Bundle>, AppException>>
{
    public readonly IDbContext dbContext;
    public readonly ILogger<GetAllBudlesHandler> logger;

    public GetAllBudlesHandler(IDbContext dbContext, ILogger<GetAllBudlesHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<List<Bundle>, AppException>> Handle(GetAllBundles request, CancellationToken cancellationToken)
    {
        var bundleResponse = await dbContext.Bundles.ListAsync();
        if (bundleResponse.IsT1) return bundleResponse.AsT1.LogAndReturnError(logger);
        var list = bundleResponse.AsT0; 
        return list;

    }
}


