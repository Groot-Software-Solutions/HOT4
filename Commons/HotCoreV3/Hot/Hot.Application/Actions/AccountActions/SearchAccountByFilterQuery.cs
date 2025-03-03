namespace Hot.Application.Actions.AccountActions;
public record SearchAccountByFilterQuery(string Filter) : IRequest<OneOf<List<Account>, NotFoundException, AppException>>;
        public class SearchAccountByFilterHandler : IRequestHandler<SearchAccountByFilterQuery, OneOf<List<Account>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<SearchAccountByFilterHandler> logger;

            public SearchAccountByFilterHandler(IDbContext context, ILogger<SearchAccountByFilterHandler> logger)
            {  
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<List<Account>, NotFoundException, AppException>> Handle(SearchAccountByFilterQuery request, CancellationToken cancellationToken)
            {

                var response = await _context.Accounts.SearchAsync(request.Filter);
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(response.AsT1.Message,logger);
                var accountList = response.AsT0;
               
                return accountList;
            }

        }
