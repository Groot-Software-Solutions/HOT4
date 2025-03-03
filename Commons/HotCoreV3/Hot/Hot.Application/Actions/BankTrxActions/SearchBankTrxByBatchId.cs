namespace Hot.Application.Actions.BankTrxActions;

public record SearchBankTrxByBatchId(long BatchId) : IRequest<OneOf<List<BankTrx>, NotFoundException, AppException>>;

public class SearchBankTrxByBatchIdHandler : IRequestHandler<SearchBankTrxByBatchId, OneOf<List<BankTrx>, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<SearchBankTrxByBatchIdHandler> _logger;

    public SearchBankTrxByBatchIdHandler(IDbContext context, ILogger<SearchBankTrxByBatchIdHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<List<BankTrx>, NotFoundException, AppException>> Handle(SearchBankTrxByBatchId request, CancellationToken cancellationToken)
    {
        var response = await _context.BankTrxs.ListAsync(request.BatchId);
        if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(_logger);
        var bankTrxList = response.AsT0;
        return bankTrxList;

    }

}

