namespace Hot.Application.Actions.AccessActions;

public record AccessResetPinCommand(int AccessId) : IRequest<OneOf<string, NotFoundException, AppException>>;

public class AccessResetPinCommandHandler : IRequestHandler<AccessResetPinCommand, OneOf<string, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<AccessResetPinCommandHandler> _logger;

    public AccessResetPinCommandHandler(IDbContext context, ILogger<AccessResetPinCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<string, NotFoundException, AppException>> Handle(AccessResetPinCommand request, CancellationToken cancellationToken)
    {
        var accessResult = await _context.Accesss.GetAsync(request.AccessId);
        if (accessResult.IsT1)
        {
            if (accessResult.AsT1.IsNotFoundException()) return accessResult.AsT1.ReturnNotFound("Access", request.AccessId.ToString());
            return accessResult.AsT1.LogAndReturnError("Reset Access Pin", _logger);
        }
        var access = accessResult.AsT0;
        access.AccessPassword = ResetPassword(access);
        var accessResetPinResult = await _context.Accesss.PasswordChangeAsync(access);
        if (accessResetPinResult.ResultOrNull() == false) return accessResetPinResult.AsT1.LogAndReturnError("Reset Access Pin", _logger);

        return accessResetPinResult.AsT0 ? access.AccessPassword : "";
    }

    private string ResetPassword(Access access)
    {
        var newPassword = "";
        if (access.ChannelID is (byte)Channels.USSD or (byte)Channels.Sms)
        { 
            newPassword = (new Random()).NextInt64().ToString()[..4];
        }
        else
        {
            newPassword = Guid.NewGuid().ToString().Replace("-", "").ToUpper()[..12];
        }

        return newPassword;
    }
}


