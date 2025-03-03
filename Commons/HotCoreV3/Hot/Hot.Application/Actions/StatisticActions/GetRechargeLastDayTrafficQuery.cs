namespace Hot.Application.Actions.StatisticActions;
public record GetRechargeLastDayTrafficQuery : IRequest<OneOf<List<StatSeries>, NotFoundException, AppException>>;
        public class GetRechargeLastDayTrafficQueryHandler : IRequestHandler<GetRechargeLastDayTrafficQuery, OneOf<List<StatSeries>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetRechargeLastDayTrafficQueryHandler> logger;

            public GetRechargeLastDayTrafficQueryHandler(IDbContext context, ILogger<GetRechargeLastDayTrafficQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<StatSeries>, NotFoundException, AppException>> Handle(GetRechargeLastDayTrafficQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.Statistics.GetRechargeTrafficLastDayAsync();
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(logger);
                var list = response.AsT0; 
                return list.ToStatSeries();
            }
        }
