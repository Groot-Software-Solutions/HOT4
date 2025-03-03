
namespace Hot.Application.Actions.RechargeActions;
public record RechargeRequestCommand
    (Brands BrandId, string TargetNumber, decimal Amount, long AccessId, Currency Currency = Currency.ZWG, string? ProductCode = "", string NotificationNumber = "", string? CustomSMS = null)
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;
public class RechargeRequestCommandHandler : IRequestHandler<RechargeRequestCommand, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
{
    private readonly IMediator mediator;
    private readonly IRechargeHandlerFactory rechargeHandlerFactory;

    public RechargeRequestCommandHandler(IMediator mediator, IRechargeHandlerFactory rechargeHandlerFactory)
    {
        this.mediator = mediator;
        this.rechargeHandlerFactory = rechargeHandlerFactory;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>> Handle(RechargeRequestCommand request, CancellationToken cancellationToken)
    {
        var brand = request.BrandId;
        if (IsHybridBrand(request.BrandId))
        {
           brand = IdentifyFinalFromHybridBrand(request.BrandId, request.TargetNumber, request.Currency); 
        }

        if (!rechargeHandlerFactory.HasService((int)brand)) 
            return new UnsupportedBrandException("Brand Not Handled", $"{brand}-{request.ProductCode}");

        return rechargeHandlerFactory.GetRechargeType((int)brand) switch
        {
            RechargeType.Airtime => await mediator.Send(new ProcessRechargeAirtimeCommand(brand, request.TargetNumber, request.Amount, request.AccessId, request.CustomSMS), cancellationToken),
            RechargeType.Utility => await mediator.Send(new ProcessRechargeUtilityCommand(brand, request.TargetNumber, request.NotificationNumber ?? "", request.Amount, request.AccessId, request.CustomSMS), cancellationToken),
            RechargeType.Data => await mediator.Send(new ProcessRechargeDataCommand(brand, request.TargetNumber, request.ProductCode ?? "", request.Amount, request.AccessId, request.CustomSMS), cancellationToken),
            _ => new UnsupportedBrandException("Brand Not Handled", $"{brand}-{request.ProductCode}")
        }; 
    }

    public static bool IsHybridBrand(Brands brandId)
    {
        if (brandId == Brands.AirtimeBrand) return true;
        if (brandId == Brands.DataBrand) return true;
        return false;
    }

    public static Brands IdentifyFinalFromHybridBrand(Brands brandId, string targetNumber, Currency currency)
    {
       switch(brandId)
        {
            case Brands.AirtimeBrand:
                switch (IdentifyNetwork(targetNumber))
                {
                    case Networks.Econet:
                    case Networks.Econet078:
                        return currency switch
                        { 
                            Currency.USD => Brands.EconetUSD,
                            _ => Brands.EconetPlatform,
                        };
                    case Networks.NetOne:
                        return currency switch
                        {
                            Currency.USD => Brands.NetoneUSD,
                            _ => Brands.EasyCall,
                        };
                    case Networks.Telecel:
                        return currency switch
                        {
                            Currency.USD => Brands.TelecelUSD,
                            _ => Brands.Juice,
                        }; 
                }
                break;
            case Brands.DataBrand:
                switch (IdentifyNetwork(targetNumber))
                {
                    case Networks.Econet:
                    case Networks.Econet078:
                        return currency switch
                        {
                            Currency.USD => Brands.EconetUSD,
                            _ => Brands.EconetData,
                        };
                    case Networks.NetOne:
                        return currency switch
                        {
                            Currency.USD => Brands.NetoneUSD,
                            _ => Brands.NetoneSocial,
                        };
                    case Networks.Telecel:
                        return currency switch
                        {
                            Currency.USD => Brands.TelecelUSD,
                            _ => Brands.Juice,
                        };
                    case Networks.Telone:
                        return currency switch
                        {
                            Currency.USD => Brands.TeloneUSD,
                            _ => Brands.TeloneBroadband,
                        };
                }
                break;
        }
        return brandId;
    }

    public static Networks? IdentifyNetwork(string targetNumber)
    {
        if (TargetRegexExpresions.EconetMobileNumberRegex().IsMatch(targetNumber)) return Networks.Econet;
        if (TargetRegexExpresions.TelecelMobileNumberRegex().IsMatch(targetNumber)) return Networks.Telecel;
        if (TargetRegexExpresions.NetoneMobileNumberRegex().IsMatch(targetNumber)) return Networks.NetOne;
        if (TargetRegexExpresions.ZESAMeterNumberRegex().IsMatch(targetNumber)) return Networks.ZESA;
        if (TargetRegexExpresions.LesothoMobileNumberRegex().IsMatch(targetNumber)) return Networks.Econet;
        if (TargetRegexExpresions.TeloneNumberRegex().IsMatch(targetNumber)) return Networks.Telone; 
        return null;
    }
}
