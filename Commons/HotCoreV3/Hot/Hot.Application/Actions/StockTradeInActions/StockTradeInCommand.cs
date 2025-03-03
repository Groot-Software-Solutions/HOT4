
namespace Hot.Application.Actions.StockTradeInActions;

public record StockTradeInCommand(long AccountId, decimal Amount, decimal Rate) : IRequest<OneOf<StockTradeInResult, AccountNotFoundException, AppException>>;
public class StockTradeInCommandHandler : IRequestHandler<StockTradeInCommand, OneOf<StockTradeInResult, AccountNotFoundException, AppException>>
{
    private readonly ILogger<StockTradeInCommandHandler> logger;
    private readonly IDbContext dbContext;

    public StockTradeInCommandHandler(ILogger<StockTradeInCommandHandler> logger, IDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    public async Task<OneOf<StockTradeInResult, AccountNotFoundException, AppException>> Handle(StockTradeInCommand request, CancellationToken cancellationToken)
    {
        var stockresponse = await dbContext.Transfers.StockTradeInBalanceAsync(request.AccountId);
        if (stockresponse.IsT1)
        {
            if (stockresponse.AsT1.IsNotFoundException()) return stockresponse.AsT1.ReturnAccountNotFound("Account", request.AccountId.ToString());
            return stockresponse.AsT1.LogAndReturnError("Stock Tradein", logger);
        }
        var balance = stockresponse.AsT0;

        var accountReposne = await dbContext.Accounts.GetAsync(request.AccountId);
        if (accountReposne.IsT1)
        {
            if (accountReposne.AsT1.IsNotFoundException()) return accountReposne.AsT1.ReturnAccountNotFound("Account", request.AccountId.ToString());
            return accountReposne.AsT1.LogAndReturnError("Stock Tradein", logger);
        }

        var account = accountReposne.AsT0;

        var tradableBalance = account.WalletBalance(WalletTypes.ZWG) > balance ? balance : account.WalletBalance(WalletTypes.ZWG);

        var paymentAmount = request.Amount * request.Rate;

        if (paymentAmount > tradableBalance)
            return new StockTradeInResult()
            {
                Result = -1 ,
                Message = "Allocation excessed for tradable ZWL balance" ,
                USDBalance = account.WalletBalance(WalletTypes.USD) ,
                ZWLBalance= account.WalletBalance(WalletTypes.ZWG)
            };

        var response = await dbContext.Transfers.StockTradeInAsync(request.AccountId, request.Amount, request.Rate);
        if (response.IsT1)
        {
            if (response.AsT1.IsNotFoundException()) return response.AsT1.ReturnAccountNotFound("Account", request.AccountId.ToString());
            return response.AsT1.LogAndReturnError("Stock Tradein Balance", logger);
        }
        return response.AsT0;
    }
}