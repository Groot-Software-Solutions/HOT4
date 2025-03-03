using Hot.Application.Actions.AccountActions;

namespace Hot.Application.Actions.SelfTopUpActions;
public record AddSelfTopUpCommand(Brands BrandId, string AccessCode, string TargetMobile, string BillerMobile, decimal Amount, Currency Currency, string? ProductCode = null)
    : IRequest<OneOf<SelfTopUpResult, FailedToRegisterUserException, SelfTopupNotAllowedForUserProfileException, InvalidWalletProviderNumberException, AppException>>;

public class AddSelfTopUpCommandHandler : IRequestHandler<AddSelfTopUpCommand, OneOf<SelfTopUpResult, FailedToRegisterUserException, SelfTopupNotAllowedForUserProfileException, InvalidWalletProviderNumberException, AppException>>
{
    private readonly IDbContext context;
    private readonly ILogger<AddSelfTopUpCommandHandler> logger;
    private readonly IMediator mediator;

    public AddSelfTopUpCommandHandler(IDbContext context, ILogger<AddSelfTopUpCommandHandler> logger, IMediator mediator)
    {
        this.context = context;
        this.logger = logger;
        this.mediator = mediator;
    }

    public async Task<OneOf<SelfTopUpResult, FailedToRegisterUserException, SelfTopupNotAllowedForUserProfileException, InvalidWalletProviderNumberException, AppException>>
        Handle(AddSelfTopUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var accessResponse = await context.Accesss.SelectCodeAsync(request.AccessCode);
            if (accessResponse.IsT1)
            {
                if (accessResponse.AsT1.IsNotFoundException() == false) return accessResponse.AsT1.LogAndReturnError(logger);
                accessResponse = await RegisterSelfTopUpUser(request, cancellationToken);
            }
            var access = accessResponse.AsT0;
            var account = await GetAccount(access);

            //if (!AccountAllowedToSelfTopUp(account))
            //    return new SelfTopupNotAllowedForUserProfileException(account.ProfileID.ToString(), "ProfileId");


            var brandId = (int)request.BrandId;
            var brandResponse = await context.Brands.GetAsync(brandId);
            if (brandResponse.IsT1) brandResponse.AsT1.LogAndReturnError(logger);
            var brand = brandResponse.AsT0;

            var discountProfile = await GetDiscountProfile(account, brandId);

            var balance = account.WalletBalance((WalletTypes)brand.WalletTypeId);
            var CostAmount = request.Amount.GetRechargeCost(discountProfile.Discount);
            var AmountToBePaid = CostAmount - balance;



            BankTrx? bankTrx = null;
            if (AmountToBePaid > 0) bankTrx = await MakeAndSaveBankTrx(request, access, AmountToBePaid);
            SelfTopUp selfTopUp = await MakeAndSaveSelfTopUp(request, access, brandId, bankTrx);
            return new SelfTopUpResult(true, bankTrx, selfTopUp);
        }
        catch (FailedToRegisterUserException ex) { return ex; }
        catch (InvalidWalletProviderNumberException ex) { return ex; }
        catch (AppException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    private async Task<Account> GetAccount(Access access)
    {
        var accountResponse = await context.Accounts.GetAsync((int)access.AccountId);
        if (accountResponse.IsT1) throw accountResponse.AsT1.LogAndReturnError(logger);
        var account = accountResponse.AsT0;
        return account;
    }

    private async Task<ProfileDiscount> GetDiscountProfile(Account account, int brandId)
    {
        var discountResponse = await context.ProfileDiscounts.DiscountAsync(account.ProfileID, brandId);
        if (discountResponse.IsT1) throw discountResponse.AsT1.LogAndReturnError(logger);
        var discountProfile = discountResponse.AsT0;
        return discountProfile;
    }

    private async Task<OneOf<Access, HotDbException>> RegisterSelfTopUpUser(AddSelfTopUpCommand request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new AccountRegistrationCommand($"SelfTopUp-{request.AccessCode}", "SelftopUp", request.AccessCode, request.AccessCode, AccountHelper.RandomPin(), "", request.AccessCode, AccountType.Selftopup, "selftopup@hot.co.zw"), cancellationToken);
        if (response.IsT1) throw response.AsT1.FailedToRegisterUser(request.AccessCode, logger);
        var accessResponse = await context.Accesss.SelectCodeAsync(request.AccessCode);
        if (accessResponse.IsT1) throw accessResponse.AsT1.LogAndReturnError(logger);
        return accessResponse;
    }

    private async Task<BankTrx> MakeAndSaveBankTrx(AddSelfTopUpCommand request, Access access, decimal AmountToBePaid)
    {
        int BankTrxTypeId = request.BillerMobile.GetBankTrxTypeId();
        var BankId = request.BillerMobile.GetBankId(request.Currency);

        BankTrxBatch bankTrxBatch = await GetBankTrxBatch(BankId);
        string ZWGRef = request.BrandId switch
        {
            Brands.ZETDC => $"ZESA-{access.AccessId}", 
            _ => $"Self-{access.AccessId}"
        };
        string USDRef = request.BrandId switch
        { 
            Brands.ZETDCUSD => $"UUSD-{access.AccessId}",
            _ => $"USD-{access.AccessId}"
        };
        var RefName = request.Currency != Currency.USD ? ZWGRef : USDRef;
        BankTrx bankTrx = GetBankTrx(bankTrxBatch.BankTrxBatchID, request.BillerMobile, AmountToBePaid, BankTrxTypeId, RefName);

        var banktrxResponse = await context.BankTrxs.AddAsync(bankTrx);
        if (banktrxResponse.IsT1) throw banktrxResponse.AsT1.LogAndReturnError(logger);
        bankTrx.BankTrxID = banktrxResponse.AsT0;
        return bankTrx;
    }

    private async Task<SelfTopUp> MakeAndSaveSelfTopUp(AddSelfTopUpCommand request, Access access, int brandId, BankTrx? bankTrx)
    {
        SelfTopUp selfTopUp = GetSelfTopUp(request, access, bankTrx, brandId);
        var selftopupResponse = await context.SelfTopUps.AddAsync(selfTopUp);
        //TODO: Mark Banktrx as Failed so that the system doesnt take the money
        if (selftopupResponse.IsT1) throw selftopupResponse.AsT1.LogAndReturnError(logger);
        selfTopUp.SelfTopUpId = selftopupResponse.AsT0;
        return selfTopUp;
    }

    private static BankTrx GetBankTrx(long BankTrxBatchID, string billerMobile, decimal AmountToBePaid, int BankTrxTypeId, string RefName)
    {
        return new()
        {
            BankTrxBatchID = BankTrxBatchID,
            Amount = AmountToBePaid,
            TrxDate = DateTime.Now,
            RefName = RefName,
            Identifier = billerMobile,
            Branch = RefName,
            BankRef = "pending",
            Balance = 0,
            BankTrxStateID = 0,
            BankTrxTypeID = (byte)BankTrxTypeId,
        };
    }

    private async Task<BankTrxBatch> GetBankTrxBatch(int BankId)
    {
        var bankTrxBatchResponse = await context.BankTrxBatches.GetCurrentBatchAsync(
           new BankTrxBatch()
           {
               BankID = BankId,
               LastUser = "SMSUser",
               BatchDate = DateTime.Now,
               BatchReference = $"SMSMerchant {DateTime.Now:dd-MMM-yyyy}"
           });
        if (bankTrxBatchResponse.IsT1) throw CommandHelper.ReturnDbException(bankTrxBatchResponse.AsT1, logger);
        var bankTrxBatch = bankTrxBatchResponse.AsT0;
        return bankTrxBatch;
    }

    private static SelfTopUp GetSelfTopUp(AddSelfTopUpCommand request, Access access, BankTrx? bankTrx, int brandId)
    {

        SelfTopUp selfTopUp = new()
        {
            StateId = bankTrx is null ? 4 : 1,
            BanktrxId = bankTrx?.BankTrxID,
            AccessId = access.AccessId,
            Amount = request.Amount,
            BrandId = brandId,
            InsertDate = DateTime.Now,
            NotificationNumber = request.TargetMobile,
            ProductCode = request.ProductCode,
            BillerNumber = request.BillerMobile,
            TargetNumber = request.TargetMobile,
            Currency = request.Currency,
        };
        return selfTopUp;
    }

    private static bool AccountAllowedToSelfTopUp(Account account)
    {
        //TODO: Use config file to load allowed Profiles
        return account.ProfileID switch
        {
            6 => true,
            40 => true,
            _ => false
        };
    }


}
