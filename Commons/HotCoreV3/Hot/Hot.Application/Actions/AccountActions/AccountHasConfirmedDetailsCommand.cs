namespace Hot.Application.Actions.AccountActions;
public record AccountHasConfirmedDetailsCommand(long AccountId) : IRequest<OneOf<bool, NotFoundException, AppException>>;
public class AccountHasConfirmedDetailsCommandHandler : IRequestHandler<AccountHasConfirmedDetailsCommand, OneOf<bool, NotFoundException, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<AccountHasConfirmedDetailsCommandHandler> logger;

    public AccountHasConfirmedDetailsCommandHandler(IDbContext dbContext, ILogger<AccountHasConfirmedDetailsCommandHandler> logger)
    {
        this.dbContext = dbContext;
        this.logger = logger;
    }

    public async Task<OneOf<bool, NotFoundException, AppException>> Handle(AccountHasConfirmedDetailsCommand request, CancellationToken cancellationToken)
    {
        if (request == null) { }
        var accountAddressResponse = await dbContext.Addresses.GetAsync(request.AccountId);
        if (accountAddressResponse.IsT1)
        {
            if (accountAddressResponse.AsT1.IsNotFoundException()) return accountAddressResponse.AsT1.ReturnNotFound("Account Id", request.AccountId.ToString());
            return accountAddressResponse.AsT1.LogAndReturnError(logger);
        }

        var address = accountAddressResponse.AsT0;
        return (address.Longitude ?? 0) == 1;
         
    }
}