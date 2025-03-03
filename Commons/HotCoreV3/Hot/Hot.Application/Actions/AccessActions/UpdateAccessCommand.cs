namespace Hot.Application.Actions.AccessActions;
public record UpdateAccessCommand(long AccessId, string AccessCode, int ChannelID, string Name) : IRequest<OneOf<Access, NotFoundException, AppException>>;

public class UpdateAccessCommandHandler : IRequestHandler<UpdateAccessCommand, OneOf<Access, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<UpdateAccessCommandHandler> logger;

    public UpdateAccessCommandHandler(IDbContext context, ILogger<UpdateAccessCommandHandler> logger)
    {
        _context = context;
        this.logger = logger;
    }

    public async Task<OneOf<Access, NotFoundException, AppException>> Handle(UpdateAccessCommand request, CancellationToken cancellationToken)
    {
        var accessResult = await _context.Accesss.GetAsync((int)request.AccessId);
        if (accessResult.IsT1)
        {
            if (accessResult.AsT1.IsNotFoundException()) return accessResult.AsT1.ReturnNotFound("Access", request.AccessId.ToString());
            return new AppException("Update Access Error", accessResult.AsT1);
        }
        var access = accessResult.AsT0;
        access.AccessCode = request.AccessCode;
        access.ChannelID = (byte)request.ChannelID;

        var updateAccessResult = await _context.Accesss.UpdateAsync(access);
        if (updateAccessResult.IsT1) return updateAccessResult.AsT1.LogAndReturnError("Update Access Error",logger);

        var accessWebResult = await _context.AccessWebs.GetAsync((int)request.AccessId);
        if (accessWebResult.IsT1)
        {
            if (accessWebResult.AsT1.IsNotFoundException())
            {
                var addAccessWebResult = await _context.AccessWebs.AddAsync(
                         new AccessWeb()
                         {
                             AccessID = access.AccessId,
                             AccessName = request.Name,
                         });
                if (addAccessWebResult.IsT1) return addAccessWebResult.AsT1.LogAndReturnError("Update Access Error", logger);
                return access;
            }
            return accessWebResult.AsT1.LogAndReturnError("Update Access Error", logger);
        }

        var accessWeb = accessWebResult.AsT0;

        accessWeb.AccessName = request.Name; 
        var updateAccessWebResult = await _context.AccessWebs.UpdateAsync(accessWeb);
        if (updateAccessWebResult.IsT1) return updateAccessWebResult.AsT1.LogAndReturnError("Update Access Error", logger);

        return access;
    }
}
