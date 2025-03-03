namespace Hot.Application.Actions.AccountActions;
public record GetAccountQuery(string AccessCode) : IRequest<OneOf<Account, NotFoundException, AppException>>;
public class GetAccountQueryHandler : IRequestHandler<GetAccountQuery, OneOf<Account, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<GetAccountQueryHandler> logger;

    public GetAccountQueryHandler(IDbContext context, ILogger<GetAccountQueryHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<Account, NotFoundException, AppException>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AccessCode)) return new NotFoundException("No Access Code provided", request.AccessCode);

        var accessResponse = await _context.Accesss.SelectCodeAsync(request.AccessCode);
        if (accessResponse.IsT1) return HandleError(request, accessResponse.AsT1);

        var access = accessResponse.AsT0;
        var accountResponse = await _context.Accounts.GetAsync((int)access.AccountId);
        if (accountResponse.IsT1) HandleError(request, accountResponse.AsT1);

        return accountResponse.AsT0;

    }

    private OneOf<Account, NotFoundException, AppException> HandleError(GetAccountQuery request, HotDbException error)
    {
        if (error.IsNotFoundException()) return new NotFoundException($"Account not found", request.AccessCode);
        return error.LogAndReturnError(logger);
    }



}
