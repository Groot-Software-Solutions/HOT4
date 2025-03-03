using Hot.Application.Actions.SMSActions;
using System.Text;

namespace Hot.Application.Actions.RechargeActions;
public record ProcessRechargeDataCommand(Brands BrandId, string TargetMobile, string ProductCode, decimal Amount, long AccessId, string? CustomSMS = null)
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;

public class ProcessRechargeDataCommandHandler : IRequestHandler<ProcessRechargeDataCommand, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException,
        UnsupportedBrandException, AppException>>
{
    private readonly IRechargeHandlerFactory rechargeHandlerFactory;
    private readonly IRechargeDataQueryHandlerFactory rechargeDataQueryHandlerFactory;
    private readonly IDbContext context;
    private readonly ILogger<ProcessRechargeCommandHandler> logger;
    private readonly IMediator mediator;

    public ProcessRechargeDataCommandHandler(IRechargeHandlerFactory rechargeHandlerFactory, IRechargeDataQueryHandlerFactory rechargeDataQueryHandlerFactory, IDbContext context, ILogger<ProcessRechargeCommandHandler> logger, IMediator mediator)
    {
        this.rechargeHandlerFactory = rechargeHandlerFactory;
        this.rechargeDataQueryHandlerFactory = rechargeDataQueryHandlerFactory;
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(ProcessRechargeDataCommand request, CancellationToken cancellationToken)
    {

        if (rechargeHandlerFactory.HasService((int)request.BrandId) == false) return new UnsupportedBrandException((int)request.BrandId, null, context);
        var rechargeService = rechargeHandlerFactory.GetService((int)request.BrandId);

        var brandResponse = await context.Brands.GetAsync((int)request.BrandId);
        if (brandResponse.IsT1) return new UnsupportedBrandException((int)request.BrandId, "Unable to get brand", context);
        var brand = brandResponse.AsT0;

        if (rechargeDataQueryHandlerFactory.HasService(brand.NetworkId) == false) return new UnsupportedBrandException((int)request.BrandId, "No Data Query Handler", context);
        var queryService = rechargeDataQueryHandlerFactory.GetService(brand.NetworkId);

        var productQueryResponse = await queryService.IsValidProduct(request.ProductCode);
        if (!productQueryResponse.IsT0) return new AppException("Error Validating ProductCode", request.ProductCode);
        if (!productQueryResponse.AsT0) return new NotAllowedToSellBrandException("Invalid Product Code", request.ProductCode);

        var productResponse = await queryService.GetBundle(request.ProductCode);
        if (productResponse.IsT0)
        {
            var product = productResponse.AsT0;
            if (product is not null)
            {
                if (request.Amount != ((decimal)product.Amount / 100M)) return new RechargeResult() { Successful = false, Message = $"Bundle price submitted doesn't match the network price.\n Request Price:{request.Amount:##0.00}\n Network Price:{product.Amount / 100:##0.00}" };
            }
        }
        var nameQueryResponse = await queryService.GetName(request.ProductCode);
        var BundleName = nameQueryResponse.IsT0 ? nameQueryResponse.AsT0 : "";

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, (int)request.BrandId, request.Amount, request.TargetMobile, context, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;
        rechargePrepaid.Data = request.ProductCode;

        var rechargeResponse = await rechargeService.ProcessAsync(recharge, rechargePrepaid);

        if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
        var result = rechargeResponse.AsT0;
        if (result.Successful)
        {
            var DealerName = response.AsT0.Item3.AccountName;
            await mediator.Send(
                new SendConfirmationSMStoCustomerDataCommand(request.TargetMobile, BundleName, recharge.Amount, DealerName, request.CustomSMS)
            , cancellationToken);
        }
        return result;
    }


}
