namespace Hot.Application.Actions.AccessActions;
public record AddAccessCommand(string UserName, long AccountID, string Password, byte ChannelId, string Name) : IRequest<OneOf<Access, NotFoundException, AppException>>;

public class AddAccessCommandHandler : IRequestHandler<AddAccessCommand, OneOf<Access, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<AddAccessCommandHandler> logger;

    public AddAccessCommandHandler(IDbContext context, ILogger<AddAccessCommandHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<Access, NotFoundException, AppException>> Handle(AddAccessCommand request, CancellationToken cancellationToken)
    {
        var accountResult = await _context.Accounts.GetAsync((int)request.AccountID);
        if (accountResult.IsT1)
        {
            if (accountResult.AsT1.IsNotFoundException()) return accountResult.AsT1.ReturnNotFound("Account", request.AccountID.ToString());
            return new AppException("Create Access Error", accountResult.AsT1);
        }
        var account = accountResult.AsT0;

        var access = new Access()
        {
            AccessCode = request.UserName,
            AccessPassword = request.Password,
            ChannelID = request.ChannelId,
            AccountId = account.AccountID,
        };
        var addAccessResult = await _context.Accesss.AddAsync(access);
        if (addAccessResult.ResultOrNull() == -1) return accountResult.AsT1.LogAndReturnError(logger);
        access.AccessId = addAccessResult.ResultOrNull();
        _ = await _context.AccessWebs.AddAsync(
            new AccessWeb()
            {
                AccessID = access.AccessId,
                AccessName = request.Name,
            });

        return access;
    }
}

