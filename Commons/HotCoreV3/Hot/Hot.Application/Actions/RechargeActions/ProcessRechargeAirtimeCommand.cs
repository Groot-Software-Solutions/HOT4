using Hot.Application.Actions.SMSActions;

namespace Hot.Application.Actions.RechargeActions;


public record ProcessRechargeAirtimeCommand(Brands BrandId, string TargetMobile, decimal Amount, long AccessId, string? CustomSMS = null)
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;

public class ProcessRechargeCommandHandler : IRequestHandler<ProcessRechargeAirtimeCommand, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException,
        UnsupportedBrandException, AppException>>
{
    private readonly IRechargeHandlerFactory serviceFactory;
    private readonly IDbContext context;
    private readonly ILogger<ProcessRechargeCommandHandler> logger;
    private readonly IMediator mediator;

    public ProcessRechargeCommandHandler(IRechargeHandlerFactory serviceFactory, IDbContext context, ILogger<ProcessRechargeCommandHandler> logger, IMediator mediator)
    {
        this.serviceFactory = serviceFactory;
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(ProcessRechargeAirtimeCommand request, CancellationToken cancellationToken)
    {
         
        var brandId = (int)request.BrandId;

        if (serviceFactory.HasService(brandId) == false) return new UnsupportedBrandException(brandId, null, context);
        var service = serviceFactory.GetService(brandId);

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, brandId, request.Amount, request.TargetMobile, context, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;

        var rechargeResponse = await service.ProcessAsync(recharge, rechargePrepaid);

        if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
        var result = rechargeResponse.AsT0;
        if (result.Successful) {
            var account = response.AsT0.Item3;
            await mediator.Send(
                 
            new SendConfirmationSMStoCustomerCommand(
                request.TargetMobile, recharge.Amount,
                result.RechargePrepaid?.FinalBalance ?? 0,account.AccountName, request.CustomSMS)
            , cancellationToken); 
        }

        return result;
    }

}
 



