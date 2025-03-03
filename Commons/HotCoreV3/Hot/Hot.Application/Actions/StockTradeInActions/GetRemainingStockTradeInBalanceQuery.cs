
namespace Hot.Application.Actions.StockTradeInActions;
public record GetRemainingStockTradeInBalanceQuery(long AccountId) : IRequest<OneOf<decimal, AccountNotFoundException, AppException>>;
public class GetRemainingStockTradeInBalanceQueryHandler :
    IRequestHandler<GetRemainingStockTradeInBalanceQuery, OneOf<decimal, AccountNotFoundException, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<GetRemainingStockTradeInBalanceQueryHandler> logger;

    public GetRemainingStockTradeInBalanceQueryHandler(IDbContext dbContext, ILogger<GetRemainingStockTradeInBalanceQueryHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<decimal, AccountNotFoundException, AppException>> Handle(GetRemainingStockTradeInBalanceQuery request, CancellationToken cancellationToken)
    {
        var response = await dbContext.Transfers.StockTradeInBalanceAsync(request.AccountId);
        if (response.IsT1)
        {
            if (response.AsT1.IsNotFoundException()) return response.AsT1.ReturnAccountNotFound("Account", request.AccountId.ToString());
            return response.AsT1.LogAndReturnError("Stock Tradein Balance", logger);
        }
        return response.AsT0;
    }
}
