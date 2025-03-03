namespace Hot.Application.Actions.SMSActions;
public record SearchByDatesQuery(DateTime StartDate, DateTime EndDate) : IRequest<OneOf<List<SMS>, NotFoundException, AppException>>;
        public class SearchByDatesQueryHandler : IRequestHandler<SearchByDatesQuery, OneOf<List<SMS>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<SearchByDatesQueryHandler> logger;

            public SearchByDatesQueryHandler(IDbContext context, ILogger<SearchByDatesQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<SMS>, NotFoundException, AppException>> Handle(SearchByDatesQuery request, CancellationToken cancellationToken)
            {

                var response = await _context.SMSs.SearchByDatesAsync(request.StartDate, request.EndDate);
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(logger);
                var smsList = response.AsT0;

                return smsList;
            }
        }
