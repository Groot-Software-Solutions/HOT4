
namespace Hot.Application.Actions.RechargeActions;

public record RechargeReservationCommand(
    Brands BrandId,
    string TargetNumber,
    decimal Amount,
    long AccessId,
    Currency Currency = Currency.ZWG,
    string? ProductCode = "",
    string NotificationNumber = "",
    string? CustomSMS = null,
    bool SendConfirmation = false
    )
    : IRequest<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>;

public class RechargeReservationCommandHandler :
    IRequestHandler<RechargeReservationCommand, OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
{
    private readonly IDbContext dbContext;
    private readonly ILogger<RechargeReservationCommandHandler> logger;
    private readonly IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory;
    private readonly IRechargeDataQueryHandlerFactory rechargeDataQueryHandlerFactory;
    private readonly IMediator mediator;
    private readonly IRechargeHandlerFactory rechargeHandlerFactory;
    public RechargeReservationCommandHandler(IDbContext dbContext, ILogger<RechargeReservationCommandHandler> logger, IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory, IRechargeDataQueryHandlerFactory rechargeDataQueryHandlerFactory, IMediator mediator, IRechargeHandlerFactory rechargeHandlerFactory)
    {
        this.dbContext = dbContext;
        this.logger = logger;
        this.rechargeUtilityQueryHandlerFactory = rechargeUtilityQueryHandlerFactory;
        this.rechargeDataQueryHandlerFactory = rechargeDataQueryHandlerFactory;
        this.mediator = mediator;
        this.rechargeHandlerFactory = rechargeHandlerFactory;
    }

    public async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
        Handle(RechargeReservationCommand request, CancellationToken cancellationToken)
    {

        var brand = request.BrandId;
        if (IsHybridBrand(request.BrandId))
        {
            brand = IdentifyFinalFromHybridBrand(request.BrandId, request.TargetNumber, request.Currency);
        }

        if (!rechargeHandlerFactory.HasService((int)brand))
            return new UnsupportedBrandException("Brand Not Handled", $"{brand}-{request.ProductCode}");

        var result = rechargeHandlerFactory.GetRechargeType((int)brand) switch
        {
            RechargeType.Airtime => await ProcessAirtime(request),
            RechargeType.Utility => await ProcessUtility(request),
            RechargeType.Data => await ProcessData(request),
            _ => new UnsupportedBrandException("Brand Not Handled", $"{brand}-{request.ProductCode}")
        };
        if (request.SendConfirmation && result.IsT0)
        {
            await SendConfirmationOfReservation(request, result.AsT0);
        }
        return result;

    }



    private async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
        ProcessData(RechargeReservationCommand request)
    {
        if (rechargeHandlerFactory.HasService((int)request.BrandId) == false) return new UnsupportedBrandException((int)request.BrandId, null, dbContext);
        var rechargeService = rechargeHandlerFactory.GetService((int)request.BrandId);

        var brandResponse = await dbContext.Brands.GetAsync((int)request.BrandId);
        if (brandResponse.IsT1) return new UnsupportedBrandException((int)request.BrandId, "Unable to get brand", dbContext);
        var brand = brandResponse.AsT0;

        if (rechargeDataQueryHandlerFactory.HasService(brand.NetworkId) == false)
            return new UnsupportedBrandException((int)request.BrandId, "No Data Query Handler", dbContext);
        var queryService = rechargeDataQueryHandlerFactory.GetService(brand.NetworkId);

        var productQueryResponse = await queryService.IsValidProduct(request.ProductCode ?? "");
        if (!productQueryResponse.IsT0) return new AppException("Error Validating ProductCode", request.ProductCode ?? "");
        if (!productQueryResponse.AsT0) return new NotAllowedToSellBrandException("Invalid Product Code", request.ProductCode ?? "");

        var productResponse = await queryService.GetBundle(request.ProductCode ?? "");
        if (productResponse.IsT0)
        {
            var product = productResponse.AsT0;
            if (product is not null)
            {
                if (request.Amount != ((decimal)product.Amount / 100M)) return new RechargeResult() { Successful = false, Message = $"Bundle price submitted doesn't match the network price.\n Request Price:{request.Amount:##0.00}\n Network Price:{product.Amount / 100:##0.00}" };
            }
        }
        var nameQueryResponse = await queryService.GetName(request.ProductCode ?? "");
        var BundleName = nameQueryResponse.IsT0 ? nameQueryResponse.AsT0 : "";

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, (int)request.BrandId, request.Amount, request.TargetNumber, dbContext, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        await AddReservationData(request, response.AsT0);
        return await GetResultResponse(request, response);


    }

    private async Task AddReservationData(RechargeReservationCommand request, Tuple<Recharge, RechargePrepaid,Account> response)
    {
        var reservation = new Reservation()
        {
            AccessID = request.AccessId,
            BrandId = request.BrandId,
            Amount = request.Amount,
            TargetNumber = request.TargetNumber,
            Currency = request.Currency,
            InsertDate = DateTime.Now,
            NotificationNumber = request.NotificationNumber,
            ProductCode = request.ProductCode ?? "",
            RechargeID = response.Item1.RechargeId,
            StateId = (byte)States.Pending,
        };
       var result =  await dbContext.Reservations.AddAsync(reservation);
       
    }

    private async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
        ProcessUtility(RechargeReservationCommand request)
    {
        if (rechargeHandlerFactory.HasService((int)request.BrandId) == false) return new UnsupportedBrandException((int)request.BrandId, null, dbContext);
        var rechargeService = rechargeHandlerFactory.GetService((int)request.BrandId);

        var brandResponse = await dbContext.Brands.GetAsync((int)request.BrandId);
        if (brandResponse.IsT1) return new UnsupportedBrandException((int)request.BrandId, "Unable to get brand", dbContext);
        var brand = brandResponse.AsT0;

        if (rechargeUtilityQueryHandlerFactory.HasService((int)brand.NetworkId) == false) return new UnsupportedBrandException((int)request.BrandId, "No Utility Query Handler", dbContext);
        var queryService = rechargeUtilityQueryHandlerFactory.GetService((int)brand.NetworkId);

        var accountQueryResponse = await queryService.AccountDetails(request.TargetNumber);
        if (!accountQueryResponse.IsT0)
        {
            if (!accountQueryResponse.IsT1) return new AccountNotFoundException(accountQueryResponse.AsT1.Message, request.TargetNumber);
            return new AppException("Error Validating AccountNumber", request.TargetNumber);
        }

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, (int)request.BrandId, request.Amount, request.TargetNumber, dbContext, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;
        rechargePrepaid.SMS = request.NotificationNumber;
        await AddReservationData(request, response.AsT0);
        return await GetResultResponse(request, response);

    }


    private async Task<OneOf<RechargeResult, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, UnsupportedBrandException, AppException>>
        ProcessAirtime(RechargeReservationCommand request)
    {
        if (rechargeHandlerFactory.HasService((int)request.BrandId) == false) return new UnsupportedBrandException((int)request.BrandId, null, dbContext);
        var service = rechargeHandlerFactory.GetService((int)request.BrandId);

        var response = await ProcessRechargeBase.SetupRecharge(request.AccessId, (int)request.BrandId, request.Amount, request.TargetNumber, dbContext, logger);
        if (response.IsT0 == false) return ProcessRechargeBase.ReturnRechargeSetupError(response);
        await AddReservationData(request, response.AsT0);
        return await GetResultResponse(request, response);

    }

    private async Task SendConfirmationOfReservation(RechargeReservationCommand request, RechargeResult asT0)
    {
        var templateResponse = await dbContext.Templates.GetAsync((int)Templates.SuccessfulReservationConfirmation);
        if (templateResponse.IsT1)
        {
            templateResponse.AsT1.LogError(logger);
            return;
        }

        var template = templateResponse.AsT0;
        var sms  = template
            .SetAmount(request.Amount)
            .SetCurrency(request.Currency)
            .SetMobile(request.TargetNumber)
            .ToSMS(request.NotificationNumber);
        var result  = await dbContext.SMSs.AddAsync(sms); 
        if (result.IsT1)
        {
            result.AsT1.LogError(logger);   
            return;
        }
    }

    private async Task<RechargeResult> GetResultResponse(RechargeReservationCommand request, OneOf<Tuple<Recharge, RechargePrepaid,Account>, AccountNotFoundException, NotAllowedToSellBrandException, InsufficientFundsException, AppException> response)
    {
        var accessResult = await dbContext.Accesss.GetAsync((int)request.AccessId);
        var access = accessResult.AsT0;

        var accountResponse = await dbContext.Accounts.GetAsync((int)access.AccountId);
        var account = accountResponse.AsT0;

        var brandResponse = await dbContext.Brands.GetAsync((int)request.BrandId);
        var brand = brandResponse.AsT0;
        var recharge = response.AsT0.Item1;
        var rechargePrepaid = response.AsT0.Item2;
        rechargePrepaid.SMS = request.NotificationNumber;
        rechargePrepaid.Data = request.ProductCode;

        RechargeResult rechargeResult = new()
        {
            Recharge = recharge,
            RechargePrepaid = rechargePrepaid,
            Successful = true,
            Message = "Reservation of recharge was sucessful",
            WalletBalance = account.WalletBalance((WalletTypes)brand.WalletTypeId),
        };
        return rechargeResult;
    }

    public static bool IsHybridBrand(Brands brandId)
    {
        if (brandId == Brands.AirtimeBrand) return true;
        if (brandId == Brands.DataBrand) return true;
        return false;
    }

    public static Brands IdentifyFinalFromHybridBrand(Brands brandId, string targetNumber, Currency currency)
    {
        switch (brandId)
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
