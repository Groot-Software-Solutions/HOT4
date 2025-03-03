namespace Hot.Application.Actions.NetworkActions;
public record GetNewBalancesQuery : IRequest<OneOf<List<NetworkBalance>, NotFoundException, AppException>>;
        public class GetNewBalancesQueryHandler : IRequestHandler<GetNewBalancesQuery, OneOf<List<NetworkBalance>, NotFoundException, AppException>>
        {
            private readonly IDbContext _context;
            private readonly ILogger<GetNewBalancesQueryHandler> logger;

            public GetNewBalancesQueryHandler(IDbContext context, ILogger<GetNewBalancesQueryHandler> logger)
            {
                _context = context;
                this.logger = logger;
            }
            public async Task<OneOf<List<NetworkBalance>, NotFoundException, AppException>> Handle(GetNewBalancesQuery request, CancellationToken cancellationToken)
            {
                var response = await _context.NetworkBalance.ListAsync();
                if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError("Network Balance", logger);
                var list = response.AsT0;

                return list;
            }
        }
