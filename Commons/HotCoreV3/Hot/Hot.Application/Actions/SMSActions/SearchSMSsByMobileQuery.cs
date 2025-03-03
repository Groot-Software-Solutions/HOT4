namespace Hot.Application.Actions.SMSActions;
public record SearchSMSsByMobileQuery(string Mobile) : IRequest<OneOf<List<SMS>, NotFoundException, AppException>>;

    public class GetSMSsByMobilQueryHandler : IRequestHandler<SearchSMSsByMobileQuery, OneOf<List<SMS>, NotFoundException, AppException>>
    {
        private readonly IDbContext _context;
        private readonly ILogger<GetSMSsByMobilQueryHandler> logger;

        public GetSMSsByMobilQueryHandler(IDbContext context, ILogger<GetSMSsByMobilQueryHandler> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public async Task<OneOf<List<SMS>, NotFoundException, AppException>> Handle(SearchSMSsByMobileQuery request, CancellationToken cancellationToken)
        {
            var response = await _context.SMSs.SearchByMobileAsync(request.Mobile);
            if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(logger);
            var smsList = response.AsT0;

            return smsList;
        }
    }
