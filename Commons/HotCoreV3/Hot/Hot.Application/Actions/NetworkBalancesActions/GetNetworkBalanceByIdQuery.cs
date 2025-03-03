namespace Hot.Application.Actions.NetworkActions;

public record GetNetworkBalanceByIdQuery(int BrandId) : IRequest<OneOf<NetworkBalance, NotFoundException, AppException>>;

public class GetNetworkBalanceByIdQueryHandler : IRequestHandler<GetNetworkBalanceByIdQuery, OneOf<NetworkBalance, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetNetworkBalanceByIdQueryHandler> _logger;

    public GetNetworkBalanceByIdQueryHandler(IDbContext context, ILogger<GetNetworkBalanceByIdQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<NetworkBalance, NotFoundException, AppException>> Handle(GetNetworkBalanceByIdQuery request, CancellationToken cancellationToken)
    {
        var Response = await _context.NetworkBalance.GetByIdAsync((int)request.BrandId);
        if (Response.ResultOrNull() == null)
        {
            var error = Response.AsT1;
            if (error.IsNotFoundException()) return error.ReturnNotFound("Network Balance",request.BrandId.ToString());
            return error.LogAndReturnError(_logger);
        }
        var networkBalance = Response.AsT0;
        return networkBalance; 
    } 
}

