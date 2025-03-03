
namespace Hot.Application.Actions.QueryCustomerActions;

public record MobileCustomerDetailsQuery(Brands BrandId, string AccountNumber)
    : IRequest<OneOf<MobileAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>;


public class MobileCustomerDetailsQueryHandler
    : IRequestHandler<MobileCustomerDetailsQuery, OneOf<MobileAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>
{
    private readonly IRechargeMobileQueryHandlerFactory rechargeUtilityQueryHandlerFactory;
    private readonly IDbContext context;

    public MobileCustomerDetailsQueryHandler(IRechargeMobileQueryHandlerFactory rechargeUtilityQueryHandlerFactory, IDbContext context)
    {
        this.rechargeUtilityQueryHandlerFactory = rechargeUtilityQueryHandlerFactory;
        this.context = context;
    }

    public async Task<OneOf<MobileAccountDetailsModel, AccountNotFoundException, UnsupportedBrandException, AppException>>
        Handle(MobileCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        var brandResponse = await context.Brands.GetAsync((int)request.BrandId);
        if (brandResponse.IsT1) return new UnsupportedBrandException((int)request.BrandId, "Unable to get brand", context);
        var brand = brandResponse.AsT0;

        if (rechargeUtilityQueryHandlerFactory.HasService((int)brand.NetworkId) == false) return new UnsupportedBrandException((int)request.BrandId, "No Utility Query Handler", context);
        var queryService = rechargeUtilityQueryHandlerFactory.GetService((int)brand.NetworkId);

        var accountQueryResponse = await queryService.AccountDetails(request.AccountNumber);
        if (!accountQueryResponse.IsT0)
        {
            if (accountQueryResponse.IsT1) return new AccountNotFoundException(accountQueryResponse.AsT1.Message, request.AccountNumber);
            return new AppException("Error Validating AccountNumber", request.AccountNumber);
        }
        return accountQueryResponse.AsT0;
    }
}
