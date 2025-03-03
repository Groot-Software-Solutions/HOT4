namespace Hot.Application.Actions.StatisticActions;
public record GetRechargeLatestTrafficQuery : IRequest<OneOf<List<StatSeries>, NotFoundException, AppException>>;
        public class GetRechargeLastestDayTrafficQueryHandler : IRequestHandler<GetRechargeLatestTrafficQuery, OneOf<List<StatSeries>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetRechargeLastestDayTrafficQueryHandler> logger;

            public GetRechargeLastestDayTrafficQueryHandler(IDbContext context, ILogger<GetRechargeLastestDayTrafficQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<StatSeries>, NotFoundException, AppException>> Handle(GetRechargeLatestTrafficQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.Statistics.GetRechargeTrafficAsync();
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(logger);
                var statResult = response.AsT0;

                return statResult.ToStatSeries();
            }
        }
