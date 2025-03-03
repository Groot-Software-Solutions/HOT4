namespace Hot.Application.Actions.BankActions;
public record GetBanksQuery : IRequest<OneOf<List<Bank>, NotFoundException, AppException>>;

public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, OneOf<List<Bank>, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetBanksQueryHandler> _logger;

    public GetBanksQueryHandler(IDbContext context, ILogger<GetBanksQueryHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<List<Bank>, NotFoundException, AppException>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
    {
        var response = await _context.Banks.ListAsync();
        if (response.IsT1) return response.AsT1.LogAndReturnError(_logger);
         
        var banksList = response.AsT0;
        if (!banksList.Any()) return new NotFoundException("Banks", "");
        return banksList;
    }
    
}
