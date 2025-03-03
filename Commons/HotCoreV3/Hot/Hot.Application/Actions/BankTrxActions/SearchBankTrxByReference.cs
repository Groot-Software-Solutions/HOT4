namespace Hot.Application.Actions.BankTrxActions;
public record SearchBankTrxByReference(string BankRef) : IRequest<OneOf<BankTrx, NotFoundException, AppException>>;

public class SearchBankTrxByReferenceHandler : IRequestHandler<SearchBankTrxByReference, OneOf<BankTrx, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<SearchBankTrxByReferenceHandler> _logger;

    public SearchBankTrxByReferenceHandler(IDbContext context, ILogger<SearchBankTrxByReferenceHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<BankTrx, NotFoundException, AppException>> Handle(SearchBankTrxByReference request, CancellationToken cancellationToken)
    {
        var Response = await _context.BankTrxs.GetByRefAsync(request.BankRef);
        if (Response.IsT1)
        {
            if (Response.AsT1.IsNotFoundException()) return Response.AsT1.ReturnNotFound("Bank transactions", request.BankRef);
            return Response.AsT1.LogAndReturnError(_logger);
        }
        var bankTrx = Response.AsT0;
        return bankTrx;
    }
}