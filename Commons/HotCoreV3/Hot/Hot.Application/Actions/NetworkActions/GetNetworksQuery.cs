namespace Hot.Application.Actions.NetworkActions
{

    public record GetNetworksQuery: IRequest<OneOf<List<Network>,  AppException>>;

    public class GetNetworksQueryHandler : IRequestHandler<GetNetworksQuery, OneOf<List<Network>,  AppException>>
    {
        private readonly IDbContext _context;
        private readonly ILogger<GetNetworksQueryHandler> _logger;

        public GetNetworksQueryHandler(IDbContext context, ILogger<GetNetworksQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<OneOf<List<Network>, AppException>> Handle(GetNetworksQuery request, CancellationToken cancellationToken)
        {
            var Response = await _context.Networks.ListAsync();
            if (Response.IsT1)
            {
                var error = Response.AsT1;
                return error.LogAndReturnError(_logger);
            }
            var networks = Response.AsT0;
            return networks;
        }
    }
}
