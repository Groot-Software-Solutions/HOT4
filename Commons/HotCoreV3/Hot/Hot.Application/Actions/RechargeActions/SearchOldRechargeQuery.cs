using AutoMapper;

namespace Hot.Application.Actions.RechargeActions
{
    public record SearchOldRechargeQuery(string Filter) : IRequest<OneOf<List<RechargeResultModel>, AppException>>;
    public class SearchOldRechargeQueryHandler : IRequestHandler<SearchOldRechargeQuery, OneOf<List<RechargeResultModel>, AppException>>
    {
        private readonly IDbContext _context;
        private readonly ILogger<SearchOldRechargeQueryHandler> logger;
        private readonly IMapper mapper;

        public SearchOldRechargeQueryHandler(IDbContext context, ILogger<SearchOldRechargeQueryHandler> logger, IMapper mapper)
        {
            _context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<OneOf<List<RechargeResultModel>, AppException>> Handle(SearchOldRechargeQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.Recharges.FindOldAsync(request.Filter);
            if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
            var rechargesList = response.AsT0;

            return rechargesList;
        }
    }
}
