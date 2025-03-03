namespace Hot.Application.Actions.LimitActions;
public record AccountLimitQuery(long AccountId, int NetworkId = 1) : IRequest<OneOf<LimitModel, AppException>>;
public class GetLimitByAccountIdQueryHandler : IRequestHandler<AccountLimitQuery, OneOf<LimitModel, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetLimitByAccountIdQueryHandler> logger;

    public GetLimitByAccountIdQueryHandler(IDbContext context, ILogger<GetLimitByAccountIdQueryHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<LimitModel, AppException>> Handle(AccountLimitQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Limits.GetCurrentLimitsAsync(request.NetworkId,request.AccountId);
        if (response.IsT0)
        {
            return response.AsT0;
        }
        return response.AsT1.LogAndReturnError(logger);
    }
}

