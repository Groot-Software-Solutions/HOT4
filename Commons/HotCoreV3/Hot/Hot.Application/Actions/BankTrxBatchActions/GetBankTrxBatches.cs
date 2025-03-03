namespace Hot.Application.Actions.BankTrxBatchActions;

public record GetBankTrxBatches(byte BankId) : IRequest<OneOf<List<BankTrxBatch>, AppException>>;

public class GetBankTrxBatchesHandler : IRequestHandler<GetBankTrxBatches, OneOf<List<BankTrxBatch>, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetBankTrxBatchesHandler> _logger;

    public GetBankTrxBatchesHandler(IDbContext context, ILogger<GetBankTrxBatchesHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<List<BankTrxBatch>, AppException>> Handle(GetBankTrxBatches request, CancellationToken cancellationToken)
    {
        var response = await _context.BankTrxBatches.ListAsync(request.BankId);
        if (response.IsT1) return response.AsT1.LogAndReturnError(_logger);
        var bankTrxBatchList = response.AsT0;

        return bankTrxBatchList;
    }
}
