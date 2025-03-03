namespace Hot.Application.Actions.AccessActions;
public record DeleteAccessCommand(int AccessId) : IRequest<OneOf<bool, NotFoundException, AppException>>;
         
public class DeleteAccessCommandHandler : IRequestHandler<DeleteAccessCommand, OneOf<bool, NotFoundException, AppException>>
{
    private readonly IDbContext _context;
    private readonly ILogger<DeleteAccessCommandHandler> _logger;

    public DeleteAccessCommandHandler(IDbContext context, ILogger<DeleteAccessCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<OneOf<bool, NotFoundException, AppException>> Handle(DeleteAccessCommand request, CancellationToken cancellationToken)
    {
        var accessResult = await _context.Accesss.GetAsync(request.AccessId);
        if (accessResult.IsT1)
        {
            if (accessResult.AsT1.IsNotFoundException()) return accessResult.AsT1.ReturnNotFound("Access", request.AccessId.ToString());
            return new AppException("Delete Access Error", accessResult.AsT1);
        }
        var access = accessResult.AsT0;
        var accessRemoveResult = await _context.Accesss.RemoveAsync((int)access.AccessId);
        if (accessRemoveResult.IsT1) return accessRemoveResult.AsT1.LogAndReturnError(_logger);

        return accessRemoveResult.IsT0;
    }

    
}
