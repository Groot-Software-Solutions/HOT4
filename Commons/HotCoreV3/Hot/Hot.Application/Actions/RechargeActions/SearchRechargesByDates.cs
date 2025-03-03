namespace Hot.Application.Actions.RechargeActions;
public record SearchRechargesByDates(long AccountId, DateTime StartDate, DateTime EndDate) : IRequest<OneOf<List<Recharge>,AppException>>;
        public class SearchRechargesByAccountCommandHandler : IRequestHandler<SearchRechargesByDates, OneOf<List<Recharge>, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<SearchRechargesByAccountCommandHandler> _logger;

            public SearchRechargesByAccountCommandHandler(IDbContext context, ILogger<SearchRechargesByAccountCommandHandler> logger)
            {
                _context = context;
                this._logger = logger;
            }
            public async Task<OneOf<List<Recharge>, AppException>> Handle(SearchRechargesByDates request, CancellationToken cancellationToken)
            {
                var response = await _context.Recharges.SelectAggregatorAsync((int)request.AccountId, request.StartDate,request.EndDate);
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(_logger);
                   var rechargeslist = response.AsT0;
                    return rechargeslist;
            }
        }
