namespace Hot.Application.Actions.AccountActions;
public record GetBalanceQuery(long AccountId) : IRequest<OneOf<decimal, AppException>>;
        public class GetBalanceQueryHandler : IRequestHandler<GetBalanceQuery, OneOf<decimal, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetBalanceQueryHandler> logger;

            public GetBalanceQueryHandler(IDbContext context, ILogger<GetBalanceQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            } 

            public async Task<OneOf<decimal, AppException>> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.Accounts.GetAsync((int)request.AccountId);
                if (response.IsT0)
                {
                    return response.AsT0.Balance;
                }
                return response.AsT1.LogAndReturnError(logger);
            }
        }