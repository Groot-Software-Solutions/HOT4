namespace Hot.Application.Actions.AccountActions;
public record GetZesaBalanceQuery(long AccountId) : IRequest<OneOf<decimal, AppException>>;
        public class GetZesaBalanceQueryHandler : IRequestHandler<GetZesaBalanceQuery, OneOf<decimal, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetZesaBalanceQueryHandler> logger;

            public GetZesaBalanceQueryHandler(IDbContext context, ILogger<GetZesaBalanceQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }

            public async Task<OneOf<decimal, AppException>> Handle(GetZesaBalanceQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.Accounts.GetAsync((int)request.AccountId);
                if (response.IsT0)
                {
                    return response.AsT0.ZesaBalance;
                }
                return response.AsT1.LogAndReturnError(logger);
            }
        }
