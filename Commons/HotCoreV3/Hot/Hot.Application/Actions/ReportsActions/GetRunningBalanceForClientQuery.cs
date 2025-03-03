using AutoMapper;

namespace Hot.Application.Actions.ReportsActions;
public record GetRunningBalanceForClientQuery(long AccountID, DateTime StartDate, DateTime EndDate) : IRequest<OneOf<List<StatementTransactionModel>, AppException>>;
public class GetRunningBalanceForClientQueryHandler : IRequestHandler<GetRunningBalanceForClientQuery, OneOf<List<StatementTransactionModel>, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<GetRunningBalanceForClientQueryHandler> logger;
    private readonly IMapper mapper;

    public GetRunningBalanceForClientQueryHandler(IDbContext context, ILogger<GetRunningBalanceForClientQueryHandler> logger, IMapper mapper)
    {
        this.context = context;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<OneOf<List<StatementTransactionModel>, AppException>> Handle(GetRunningBalanceForClientQuery request, CancellationToken cancellationToken)
    {
        decimal startingBalance = 0;
        var response = await context.Report.GetStatementAsync(request.AccountID, request.StartDate, request.EndDate);
        if (response.IsT1) return response.AsT1.LogAndReturnError(logger);
        var transactionsList = response.AsT0;

        var startingBalanceResult = await context.Report.GetStartingBalanceAsync(request.AccountID, request.StartDate);
        if (startingBalanceResult.IsT0) startingBalance = startingBalanceResult.AsT0;

        var transactionModelsList = transactionsList.Select(t =>mapper.Map<StatementTransactionModel>(t)).ToList();
        foreach (var transactionModel in transactionModelsList)
        { 
            transactionModel.Balance = startingBalance - 
                ((transactionModel.TranType == "Recharge") 
                ? transactionModel.Cost 
                : - transactionModel.Amount);
            startingBalance = transactionModel.Balance;
        }
        return transactionModelsList;
    }

    


}


