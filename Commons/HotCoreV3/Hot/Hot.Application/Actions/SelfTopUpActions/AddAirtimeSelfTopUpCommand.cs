
using Hot.Application.Actions.RechargeActions; 

namespace Hot.Application.Actions.SelfTopUpActions;
public record AddAirtimeSelfTopUpCommand(string AccessCode, string TargetMobile, string BillerMobile, decimal Amount, Currency Currency)
    : IRequest<OneOf<SelfTopUpResult, FailedToRegisterUserException, SelfTopupNotAllowedForUserProfileException, InvalidWalletProviderNumberException, AppException>>;

public class AddAirtimeSelfTopUpCommandHandler : IRequestHandler<AddAirtimeSelfTopUpCommand, OneOf<SelfTopUpResult, FailedToRegisterUserException, SelfTopupNotAllowedForUserProfileException, InvalidWalletProviderNumberException, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<AddAirtimeSelfTopUpCommandHandler> logger;
    private readonly IMediator mediator;

    public AddAirtimeSelfTopUpCommandHandler(IDbContext context, ILogger<AddAirtimeSelfTopUpCommandHandler> logger, IMediator mediator)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<SelfTopUpResult, FailedToRegisterUserException, SelfTopupNotAllowedForUserProfileException, InvalidWalletProviderNumberException, AppException>> Handle(AddAirtimeSelfTopUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var brandId = await GetTransactionBrandId(request.TargetMobile, request.Currency);
            var command = new AddSelfTopUpCommand((Brands)brandId, request.AccessCode, request.TargetMobile,
                request.BillerMobile, request.Amount, request.Currency, ""); 
            return await mediator.Send( command , cancellationToken);            
        } 
        catch (AppException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    } 

    private async Task<int> GetTransactionBrandId(string TargetMobile, Currency currency)
    {
        var brandResponse = await ProcessRechargeBase.GetBrandId(
            TargetMobile.ToTargetMobileWithSuffix(currency), context, logger);
        if (brandResponse.IsT1) throw brandResponse.AsT1.LogAndReturnError(logger);
        return brandResponse.AsT0;
    }
     
}
