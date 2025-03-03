namespace Hot.Application.Actions.RechargeActions;

public record ProcessPinRechargeCommand(string TargetMobile, decimal Amount, long AccessId, int BrandId = 0)
: IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;
public class ProcessPinRechargeCommandHandler : IRequestHandler<ProcessPinRechargeCommand,
    OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
{
    private readonly IRechargeHandlerFactory serviceFactory;
    private readonly IDbContext context;
    private readonly ILogger<ProcessPinRechargeCommandHandler> logger;

    public ProcessPinRechargeCommandHandler(IRechargeHandlerFactory serviceFactory, IDbContext context, ILogger<ProcessPinRechargeCommandHandler> logger)
    {
        this.serviceFactory = serviceFactory;
        this.context = context;
        this.logger = logger;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(ProcessPinRechargeCommand request, CancellationToken cancellationToken)
    {
        var service = serviceFactory.GetService(0);

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, request.BrandId, request.Amount, request.TargetMobile, context, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;

        var rechargeResponse = await service.ProcessAsync(recharge, rechargePrepaid);

        if (rechargeResponse.IsT1) return rechargeResponse.AsT1.LogAndReturnError(logger);
        var result = rechargeResponse.AsT0;

        if (result.Successful)
        {
            await HandleSuccess(result);

        }

        return result;
    }


    private async Task HandleSuccess(RechargeResult rechargeResult)
    {
        List<Pin> pins = (List<Pin>)(rechargeResult.Data ?? new());
        if (rechargeResult.Recharge is null) return;
        var recharge = rechargeResult.Recharge;
        var accessResponse = await context.Accesss.GetAsync((int)(recharge.AccessId));
        var accountResponse = await context.Accounts.GetAsync((int)accessResponse.AsT0.AccountId);
        if (accessResponse.IsT0)
        {
            if (accessResponse.AsT0.ChannelID == (int)Channels.USSD || accessResponse.AsT0.ChannelID == (int)Channels.Sms)
            {
                var brandResponse = await context.Brands.GetAsync(recharge.BrandId);

                List<Template> dealerReplies = new();
                Template dealerheader = (await context.Templates.GetAsync((int)Templates.SuccessfulRecharge_Dealer_Header)).AsT0;

                dealerheader
                    .SetMobile(recharge.Mobile)
                    .SetAmount(recharge.Amount)
                    .SetDiscount(recharge.Discount)
                    .SetCost(recharge.Amount.GetRechargeCost(recharge.Discount))
                    .SetBalance(accountResponse.AsT0.Balance)
                    .SetSaleValue(accountResponse.AsT0.SaleValue);
                dealerReplies.Add(dealerheader);

                foreach (var pin in pins)
                {
                    Template pinReply = (await context.Templates.GetAsync((int)Templates.SuccessfulRechargePin_Dealer)).AsT0;
                    pinReply
                        .SetPin(pin.PinNumber)
                        .SetPinRef(pin.PinRef)
                        .SetPinValue(pin.PinValue)
                        .SetMobile(recharge.Mobile)
                        .SetBrand(brandResponse.AsT0.BrandName);
                    dealerReplies.Add(pinReply);
                }
                await SendMessages(accessResponse.AsT0.AccessCode, dealerReplies);
            }

        }

        List<Template> CustomerReplies = new();
        foreach (var pin in pins)
        {
            Template pinReply = new() { TemplateText = "*121*%PIN%#" };
            pinReply.SetPin(pin.PinNumber);
            CustomerReplies.Add(pinReply);
        }
         
        Template customerHeader = (await context.Templates.GetAsync((int)Templates.SuccessfulRechargePin_Customer_Header)).AsT0;
        customerHeader.SetAmount(recharge.Amount);
        CustomerReplies.Add(customerHeader);

        await SendMessages(recharge.Mobile, CustomerReplies);

    }

    private async Task SendMessages(string TargetMobile, List<Template> templates)
    {
        foreach (var item in templates)
        {
            SMS sMS = new()
            {
                Direction = false,
                Mobile = TargetMobile,
                Priority = new() { PriorityId = (int)Priorities.Normal },
                State = new() { StateID = (int)States.Pending },
                SMSText = item.TemplateText,
                SMSDate = DateTime.Now,
                InsertDate = DateTime.Now,
            };
            _ = await context.SMSs.AddAsync(sMS);
        }
    }

}
