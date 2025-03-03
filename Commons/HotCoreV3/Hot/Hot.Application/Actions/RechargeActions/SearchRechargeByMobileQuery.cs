using AutoMapper;

namespace Hot.Application.Actions.RechargeActions;
public record SearchRechargeByMobileQuery(string Mobile) : IRequest<OneOf<List<RechargeResultModel>, AppException>>;
public class SearchRechargeByMobileQueryHandler : IRequestHandler<SearchRechargeByMobileQuery, OneOf<List<RechargeResultModel>, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<SearchRechargeByMobileQueryHandler> logger;
    private readonly IMapper mapper;

    public SearchRechargeByMobileQueryHandler(IDbContext context, ILogger<SearchRechargeByMobileQueryHandler> logger, IMapper mapper)
    {
        _context = context;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<OneOf<List<RechargeResultModel>, AppException>> Handle(SearchRechargeByMobileQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Recharges.FindByMobileAsync(request.Mobile);
        if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
        var rechargesList = response.AsT0;

        return rechargesList;
    }
}
