using Hot.Application.Actions.RechargeActions;

namespace Hot.Application.Actions.SelfTopUpActions;
public record ProcessPendingSelfTopUpCommand(SelfTopUp Item) : IRequest<OneOf<SelfTopUpResult, AppException>>;
public class ProcessPendingSelfTopUpCommandHandler : IRequestHandler<ProcessPendingSelfTopUpCommand, OneOf<SelfTopUpResult, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<ProcessPendingSelfTopUpCommand> logger;
    private readonly IMediator mediator;
    public ProcessPendingSelfTopUpCommandHandler(IMediator mediator, IDbContext context, ILogger<ProcessPendingSelfTopUpCommand> logger)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<SelfTopUpResult, AppException>> Handle(ProcessPendingSelfTopUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await mediator.Send(
                    new RechargeRequestCommand(
                        (Brands)request.Item.BrandId,
                        request.Item.TargetNumber,
                        request.Item.Amount,
                        request.Item.AccessId,
                        request.Item.Currency,
                        request.Item.ProductCode,
                        request.Item.NotificationNumber ?? ""), cancellationToken);
            if (response.IsT0) return await HandleSuccessAsync(request, response);
            return await HandleFailureAsync(request);
        }
        catch (Exception ex)
        {
            return ex.LogAndReturnError(logger);
        }
    }

    private async Task<SelfTopUpResult> HandleFailureAsync(ProcessPendingSelfTopUpCommand request)
    {
        request.Item.StateId = (int)States.Failure;
        _ = await context.SelfTopUps.UpdateAsync(request.Item);
        return new SelfTopUpResult(false, null, request.Item);
    }

    private async Task<SelfTopUpResult> HandleSuccessAsync(ProcessPendingSelfTopUpCommand request,
        OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException> response)
    {
        var rechargeResult = response.AsT0;
        request.Item.StateId = rechargeResult.Recharge?.StateId ?? 0;
        request.Item.RechargeId = rechargeResult.Recharge?.RechargeId ?? 0;
        _ = await context.SelfTopUps.UpdateAsync(request.Item);
        return new SelfTopUpResult(rechargeResult.Successful, rechargeResult.Recharge);
    }

}

