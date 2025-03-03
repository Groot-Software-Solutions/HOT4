namespace Hot.Application.Actions.BankTrxActions;

public record CreateBankTrxCommand(BankTrx BankTrx) : IRequest<OneOf<BankTrx, AppException>>;

public class CreateBatchTrxHandler : IRequestHandler<CreateBankTrxCommand, OneOf<BankTrx, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<CreateBatchTrxHandler> logger;

    public CreateBatchTrxHandler(IDbContext context, ILogger<CreateBatchTrxHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<BankTrx, AppException>> Handle(CreateBankTrxCommand request, CancellationToken cancellationToken)
    {
        var bankTrx = request.BankTrx;

        var result = await _context.BankTrxs.AddAsync(bankTrx);
        if (result.ResultOrNull() == -1) return result.AsT1.LogAndReturnError("Error adding BankTrx", logger);
        bankTrx.BankTrxID = result.AsT0;
        return bankTrx;
    }
}

