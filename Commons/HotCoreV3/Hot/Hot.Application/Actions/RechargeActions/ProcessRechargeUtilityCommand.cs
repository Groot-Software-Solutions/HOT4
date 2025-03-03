using Hot.Application.Actions.SMSActions;
using Hot.Application.Common.Models.RechargeServiceModels.ZESA;
using System.Text.Json;

namespace Hot.Application.Actions.RechargeActions;
public record ProcessRechargeUtilityCommand(Brands BrandId, string AccountNumber, string NotificationNumber, decimal Amount, long AccessId, string? CustomSMS = null)
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;

public class ProcessRechargeUtilityCommandHandler : IRequestHandler<ProcessRechargeUtilityCommand, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException,
        UnsupportedBrandException, AppException>>
{
    private readonly IRechargeHandlerFactory rechargeHandlerFactory;
    private readonly IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory;
    private readonly IDbContext context;
    private readonly ILogger<ProcessRechargeUtilityCommandHandler> logger;
    private readonly IMediator mediator;

    public ProcessRechargeUtilityCommandHandler(IRechargeHandlerFactory rechargeHandlerFactory, IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory, IDbContext context, ILogger<ProcessRechargeUtilityCommandHandler> logger, IMediator mediator)
    {
        this.rechargeHandlerFactory = rechargeHandlerFactory;
        this.rechargeUtilityQueryHandlerFactory = rechargeUtilityQueryHandlerFactory;
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(ProcessRechargeUtilityCommand request, CancellationToken cancellationToken)
    {

        if (rechargeHandlerFactory.HasService((int)request.BrandId) == false) return new UnsupportedBrandException((int)request.BrandId, null, context);
        var rechargeService = rechargeHandlerFactory.GetService((int)request.BrandId);

        var brandResponse = await context.Brands.GetAsync((int)request.BrandId);
        if (brandResponse.IsT1) return new UnsupportedBrandException((int)request.BrandId, "Unable to get brand", context);
        var brand = brandResponse.AsT0;

        if (rechargeUtilityQueryHandlerFactory.HasService((int)brand.NetworkId) == false) return new UnsupportedBrandException((int)request.BrandId, "No Utility Query Handler", context);
        var queryService = rechargeUtilityQueryHandlerFactory.GetService((int)brand.NetworkId);

        var accountQueryResponse = await queryService.AccountDetails(request.AccountNumber);
        if (!accountQueryResponse.IsT0)
        {
            if (!accountQueryResponse.IsT1) return new AccountNotFoundException(accountQueryResponse.AsT1.Message, request.AccountNumber);
            return new AppException("Error Validating AccountNumber", request.AccountNumber);
        }

        var account = accountQueryResponse.AsT0;


        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, (int)request.BrandId, request.Amount, request.AccountNumber, context, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;
        rechargePrepaid.SMS = request.NotificationNumber;

        var rechargeResponse = await rechargeService.ProcessAsync(recharge, rechargePrepaid);

        if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
        var result = rechargeResponse.AsT0;
        if (result.Successful)
        {
            double units = 0;
            if (result.Data is not null)
            {
                if (result.Data is ZESAPurchaseTokenResult purchaseTokenResult)
                {
                    var token = purchaseTokenResult.PurchaseToken.Tokens.FirstOrDefault();
                    if (token is not null)  units = token.Units; 
                } 
            } 

            var senderaccount = response.AsT0.Item3;
            await mediator.Send(
              new SendConfirmationSMStoCustomerUtilityCommand(
                  request.NotificationNumber, recharge.Amount,
                  result.RechargePrepaid?.FinalBalance ?? 0, senderaccount.AccountName,
                  account.CustomerName, account.AccountNumber, (Networks)brand.NetworkId,
                  (decimal)units, request.CustomSMS)
              , cancellationToken);
        }
        return result;
    }


}
