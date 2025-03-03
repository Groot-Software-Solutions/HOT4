namespace Hot.Application.Actions.SMSActions;
public record SearchSMSsByFilterQuery(string Filter) : IRequest<OneOf<List<SMS>, NotFoundException, AppException>>;
        public class SearchSMSsByFilterQueryHandler : IRequestHandler<SearchSMSsByFilterQuery, OneOf<List<SMS>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<SearchSMSsByFilterQueryHandler> logger;

            public SearchSMSsByFilterQueryHandler(IDbContext context, ILogger<SearchSMSsByFilterQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<SMS>, NotFoundException, AppException>> Handle(SearchSMSsByFilterQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.SMSs.SearchByFilterAsync(request.Filter);
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(logger);
                var smsList = response.AsT0;

                return smsList;
            }
        }    
