


namespace Hot.Application.Actions.QueryCustomerActions;

public record QueryCustomerDetailsQuery(Brands BrandId, string AccountNumber) :
    IRequest<OneOf<CustomerDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>;
public class QueryCustomerDetailsQueryHandler : IRequestHandler<QueryCustomerDetailsQuery, OneOf<CustomerDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>
{
    private readonly IMediator mediator;
    private readonly IRechargeHandlerFactory rechargeHandlerFactory;

    public QueryCustomerDetailsQueryHandler(IMediator mediator, IRechargeHandlerFactory rechargeHandlerFactory)
    {
        this.mediator = mediator;
        this.rechargeHandlerFactory = rechargeHandlerFactory;
    }

    public async Task<OneOf<CustomerDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>> Handle(QueryCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        if (!rechargeHandlerFactory.HasService((int)request.BrandId))
            return new UnsupportedBrandException("Brand Not Handled", $"{request.BrandId}");

        return rechargeHandlerFactory.GetRechargeType((int)request.BrandId) switch
        {
            RechargeType.Airtime => MapToResult((OneOf<MobileAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>)await mediator.Send(new MobileCustomerDetailsQuery(request.BrandId, request.AccountNumber), cancellationToken)),
            RechargeType.Utility => MapToResult((OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>)await mediator.Send(new UtilityCustomerDetailsQuery(request.BrandId, request.AccountNumber), cancellationToken)),
            RechargeType.Data => await HandleDataCustomerQuery(request, cancellationToken),
            _ => new UnsupportedBrandException("Brand Not Handled", $"{request.BrandId}")
        };
    }

    private async Task<OneOf<CustomerDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>> HandleDataCustomerQuery(QueryCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        if (request.BrandId == Brands.NetoneUSD)
            return MapToResult(await mediator.Send(new MobileCustomerDetailsQuery(request.BrandId, request.AccountNumber), cancellationToken));
        return MapToResult(await mediator.Send(new UtilityCustomerDetailsQuery(request.BrandId, request.AccountNumber), cancellationToken));
    }

    private OneOf<CustomerDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException> MapToResult(OneOf<MobileAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException> response)
    {
        if (response.IsT0) return MapToCustomer(response.AsT0);
        if (response.IsT1) return response.AsT1;
        if (response.IsT2) return response.AsT2;
        return response.AsT3;
    }

    private OneOf<CustomerDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException> MapToResult(OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException> response)
    {
        if (response.IsT0) return MapToCustomer(response.AsT0);
        if (response.IsT1) return response.AsT1;
        if (response.IsT2) return response.AsT2;
        return response.AsT3;
    }

    private static CustomerDetailsModel MapToCustomer(MobileAccountDetailsModel result)
    {
        return new()
        {
            AccountName = result.CustomerName,
            Currency = Currency.ZWG,
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            Arrears = result.Arears,
            Status = result.Status
        };
    }
    private static CustomerDetailsModel MapToCustomer(UtilityAccountDetailsModel result)
    {
        return new()
        {
            AccountName = result.CustomerName,
            Currency = result.Currency,
            AccountNumber = result.AccountNumber,
            Balance = result.Balance,
            Arrears = result.Arears,
            Status = result.Status,
            Address = result.Address ?? "",
            Message = result.Message,
        };
    }
}
