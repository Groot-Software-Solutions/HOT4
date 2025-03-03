namespace Hot.Application.Actions.AccountActions;

public record GetUSDBalanceQuery(long AccountId) : IRequest<OneOf<decimal, AppException>>;

public class GetUSDBalanceQueryHandler : IRequestHandler<GetUSDBalanceQuery, OneOf<decimal, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetUSDBalanceQueryHandler> logger;

    public GetUSDBalanceQueryHandler(IDbContext context, ILogger<GetUSDBalanceQueryHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<decimal, AppException>> Handle(GetUSDBalanceQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Accounts.GetAsync((int)request.AccountId);
        if (response.IsT0)
        {
            return response.AsT0.USDBalance;
        }
        return response.AsT1.LogAndReturnError(logger);
    }
}
