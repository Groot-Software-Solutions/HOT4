namespace Hot.Application.Actions.RechargeActions;

public record SearchRechargeByAccountQuery(long AccountId, string Mobile) : IRequest<OneOf<List<Recharge>, AppException>>;

public class SearchRechargeByAccountQueryHandler : IRequestHandler<SearchRechargeByAccountQuery, OneOf<List<Recharge>, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<SearchRechargeByAccountQueryHandler> _logger;

    public SearchRechargeByAccountQueryHandler(IDbContext context, ILogger<SearchRechargeByAccountQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
     
    public async Task<OneOf<List<Recharge>, AppException>> Handle(SearchRechargeByAccountQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Recharges.FindByAccountAsync(request.AccountId, request.Mobile);
        if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(_logger);
        var rechargesList = response.AsT0;
        return rechargesList;
    }

}


