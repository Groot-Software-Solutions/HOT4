
namespace Hot.Application.Actions.SelfTopUpActions;

public record HandlePendingSelfTopUpsCommand() : IRequest<OneOf<List<SelfTopUpResult>, AppException>>;

public class HandlePendingSelfTopUpsCommandHandler : IRequestHandler<HandlePendingSelfTopUpsCommand, OneOf<List<SelfTopUpResult>, AppException>>
{
    private readonly ILogger<HandlePendingSelfTopUpsCommandHandler> logger;
    private readonly IDbContext dbContext;
    private readonly IMediator mediator;

    public HandlePendingSelfTopUpsCommandHandler(ILogger<HandlePendingSelfTopUpsCommandHandler> logger, IDbContext dbContext, IMediator mediator)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.mediator = mediator;
    }

    public async Task<OneOf<List<SelfTopUpResult>, AppException>> Handle(HandlePendingSelfTopUpsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var pendingResponse = await dbContext.SelfTopUps.ListPendingRechargeAsync();
            if (pendingResponse.IsT1) return pendingResponse.AsT1.LogAndReturnError(logger);
            var pendingSelfTopUps = pendingResponse.AsT0;

            List<SelfTopUpResult> result = new();
            foreach (var selfTop in pendingSelfTopUps)
            {
                var response = await mediator.Send(new ProcessPendingSelfTopUpCommand(selfTop), cancellationToken);
                if (response.IsT1) response.AsT1.LogError(logger);
                if (response.IsT0) result.Add(response.AsT0);
            }
            return result;
        }
        catch (Exception ex)
        {
            return ex.LogAndReturnError(logger);
        }
    }
}

