namespace Hot.Application.Actions.AccountActions;
public record AccountSearchQuery(string Search) : IRequest<OneOf<List<Account>, NotFoundException, AppException>>;
public class AccountSearchQueryHandler : IRequestHandler<AccountSearchQuery, OneOf<List<Account>, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<AccountSearchQueryHandler> logger;

    public AccountSearchQueryHandler(IDbContext context, ILogger<AccountSearchQueryHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<List<Account>, NotFoundException, AppException>> Handle(AccountSearchQuery request, CancellationToken cancellationToken)
    {

        var response = await _context.Accounts.SearchAsync(request.Search);
        if (response.ResultOrNull() == null) return response.AsT1.LogAndReturnError(response.AsT1.Message, logger);
        var accountList = response.AsT0;

        return accountList;
    }

}
