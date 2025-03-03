namespace Hot.Application.Actions.StatisticActions;
public record GetSmsTrafficQuery : IRequest<OneOf<List<StatSeries>, NotFoundException, AppException>>;
        public class GetSmsTrafficQueryHandler : IRequestHandler<GetSmsTrafficQuery, OneOf<List<StatSeries>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetSmsTrafficQueryHandler> logger;

            public GetSmsTrafficQueryHandler(IDbContext context, ILogger<GetSmsTrafficQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<StatSeries>, NotFoundException, AppException>> Handle(GetSmsTrafficQuery request, CancellationToken cancellationToken)
            {

                var response = await _context.Statistics.GetSmsTrafficAsync();
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(logger);
                var statResult = response.AsT0;

                return statResult.ToStatSeries();
            }
        }
