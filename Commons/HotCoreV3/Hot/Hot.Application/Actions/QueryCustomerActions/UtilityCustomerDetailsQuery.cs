
namespace Hot.Application.Actions.QueryCustomerActions;

public record UtilityCustomerDetailsQuery(Brands BrandId, string AccountNumber) 
    : IRequest< OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>;
public class UtilityCustomerDetailsQueryHandler 
    : IRequestHandler<UtilityCustomerDetailsQuery, OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>
{
    private readonly IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory; 
    private readonly IDbContext context;
     
    public UtilityCustomerDetailsQueryHandler(IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory, IDbContext context)
    {
        this.rechargeUtilityQueryHandlerFactory = rechargeUtilityQueryHandlerFactory;
        this.context = context;
    }

    public async Task<OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>> 
        Handle(UtilityCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        var brandResponse = await context.Brands.GetAsync((int)request.BrandId);
        if (brandResponse.IsT1) return new UnsupportedBrandException((int)request.BrandId, "Unable to get brand", context);
        var brand = brandResponse.AsT0;

        if (rechargeUtilityQueryHandlerFactory.HasService((int)brand.NetworkId) == false) return new UnsupportedBrandException((int)request.BrandId, "No Utility Query Handler", context);
        var queryService = rechargeUtilityQueryHandlerFactory.GetService((int)brand.NetworkId);

        var accountQueryResponse = await queryService.AccountDetails(request.AccountNumber);
        if (!accountQueryResponse.IsT0)
        {
            if (accountQueryResponse.IsT1) return new AccountNotFoundException(accountQueryResponse.AsT1.Message);
            return new AppException("Error Validating AccountNumber", request.AccountNumber);
        } 
       return accountQueryResponse.AsT0; 
    }
}

public record DataCustomerDetailsQuery(Brands BrandId, string AccountNumber)
    : IRequest<OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>;
public class DataCustomerDetailsQueryHandler
    : IRequestHandler<UtilityCustomerDetailsQuery, OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>
{
    private readonly IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory;
    private readonly IDbContext context;

    public DataCustomerDetailsQueryHandler(IRechargeUtilityQueryHandlerFactory rechargeUtilityQueryHandlerFactory, IDbContext context)
    {
        this.rechargeUtilityQueryHandlerFactory = rechargeUtilityQueryHandlerFactory;
        this.context = context;
    }

    public async Task<OneOf<UtilityAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>
        Handle(UtilityCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
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
        return accountQueryResponse.AsT0;
    }
}
