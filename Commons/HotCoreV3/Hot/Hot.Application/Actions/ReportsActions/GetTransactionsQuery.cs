namespace Hot.Application.Actions.ReportsActions;

public record GetTransactionsQuery(DateTime StartDate, DateTime EndDate, int ReportTypeId, long? AccountId, int? WalletTypeId, int? BankId) : IRequest<OneOf<List<StatementTransaction>, AppException>>;
public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, OneOf<List<StatementTransaction>, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<GetTransactionsQueryHandler> logger;

    public GetTransactionsQueryHandler(IDbContext context, ILogger<GetTransactionsQueryHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<OneOf<List<StatementTransaction>, AppException>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var response = await context.Report.GetTransactionsAsync(request.StartDate, request.EndDate, request.ReportTypeId, request.AccountId, request.WalletTypeId, request.BankId);
        if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
        var TransactionsList = response.AsT0;

        return TransactionsList;
    }

}
