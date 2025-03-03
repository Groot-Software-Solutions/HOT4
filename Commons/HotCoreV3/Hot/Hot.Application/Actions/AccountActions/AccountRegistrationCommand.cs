using System.Data;
using Microsoft.Extensions.Configuration;
using Hot.Application.Common.Models.Options;

namespace Hot.Application.Actions.AccountActions;

public record AccountRegistrationCommand
    (string AccountName, string Firstname, string Lastname,
    string Username, string Password, string IDNumber,
    string ReferredBy, AccountType AccountType, string Sender)
    : IRequest<OneOf<bool, AppException>>;

public class AccountRegistrationCommandHandler : IRequestHandler<AccountRegistrationCommand, OneOf<bool, AppException>>
{
    private IDbContext Context { get; set; }
    private IDbHelper DbHelper { get; set; } 
    private AppException? AppException { get; set; }
    private readonly DefaultRegistrationOptions? registrationOptions;

    public AccountRegistrationCommandHandler(IDbContext context, IDbHelper dbHelper, IConfiguration configuration)
    {
        Context = context;
        DbHelper = dbHelper;
        registrationOptions = configuration.GetSection("RegistrationDefaults").Get<DefaultRegistrationOptions>();
    }

    public async Task<OneOf<bool, AppException>> Handle(AccountRegistrationCommand request, CancellationToken cancellationToken)
    {

        if (!IsAllowedToRegisterAccounts(request.Sender)) return ApplicationError("Unauthorised");

        var existingaccess = await Context.Accesss.SelectCodeAsync(request.Username);
        if (existingaccess.IsT0) return ApplicationError("Username already registered");
         
        var dbtransaction = await BeginTransaction();
        if (dbtransaction == null) return ApplicationError("Data Connection error");

        Account account = await CreateAccount(request, dbtransaction);
        if (account.AccountID == -1) return RollbackAndReturnAppError(dbtransaction);

        bool addressresult = await CreateAddress(dbtransaction, account);
        if (addressresult == false) return RollbackAndReturnAppError(dbtransaction);

        Access access = await CreateAccess(request, dbtransaction, account);
        if (access.AccessId == -1) return RollbackAndReturnAppError(dbtransaction);

        bool accesswebresult = await CreateAccessWeb(dbtransaction, account, access);
        if (accesswebresult == false) return RollbackAndReturnAppError(dbtransaction);

        bool result = CompleteTransaction(dbtransaction);
        if (result == false) return RollbackAndReturnAppError(dbtransaction);

        return true;
    }

    private static bool IsAllowedToRegisterAccounts(string sender)
    {
        return sender.EndsWith("@hot.co.zw");
        //return true;
    }

    private bool CompleteTransaction(Tuple<IDbConnection, IDbTransaction> dbtransaction)
    {
        return DbHelper.CommitTransaction(dbtransaction.Item2).ResultOrNull();
    }

    private async Task<Tuple<IDbConnection, IDbTransaction>> BeginTransaction()
    {
        var result = await DbHelper.BeginTransaction();
        if (result.IsT1)
        {
            AppException = new AppException("DB Error attempting to create transaction", result.AsT1.Message);
        }
        return result.AsT0;
    }

    private async Task<bool> CreateAccessWeb(Tuple<IDbConnection, IDbTransaction> dbtransaction, Account account, Access access)
    {
        var accessweb = new AccessWeb()
        {
            AccessID = access.AccessId,
            AccessName = account.AccountName,
            WebBackground = "",
            SalesPassword = false,
            ResetToken = ""
        };
        var result = await Context.AccessWebs.UpdateAsync(accessweb, dbtransaction.Item1, dbtransaction.Item2);
        if (result.ResultOrNull() == false) AppException = new AppException("Error creating Accessweb Item", result.AsT1.Message);

        var accesswebresult = result.ResultOrNull();
        return accesswebresult;
    }

    private async Task<Access> CreateAccess(AccountRegistrationCommand request, Tuple<IDbConnection, IDbTransaction> dbtransaction, Account account)
    {
        var access = new Access()
        {
            AccessCode = request.Username,
            AccessPassword = request.Password,
            ChannelID = GetChannelId(request.AccountType),
            AccountId = account.AccountID,
        };
        var result = await Context.Accesss.AddAsync(access, dbtransaction.Item1, dbtransaction.Item2);
        if (result.ResultOrNull() == -1) AppException = new AppException("Error creating Access Item", result.AsT1.Message);
        access.AccessId = result.ResultOrNull();
        return access;
    }

    private async Task<bool> CreateAddress(Tuple<IDbConnection, IDbTransaction> dbtransaction, Account account)
    {
        var address = new Address()
        {
            AccountID = account.AccountID,
        };
        var result = await Context.Addresses.UpdateAsync(address, dbtransaction.Item1, dbtransaction.Item2);
        if (result.ResultOrNull() == false) AppException = new AppException("Error creating Address Item", result.AsT1.Message);
        var addressresult = result.ResultOrNull();
        return addressresult;
    }

    private async Task<Account> CreateAccount(AccountRegistrationCommand request, Tuple<IDbConnection, IDbTransaction> dbtransaction)
    {
        var account = new Account()
        {
            AccountName = GetAccountName(request),
            Email = GetEmailAddress(request),
            NationalID = request.IDNumber,
            ProfileID = GetDefaultProfileID(request.AccountType),
            ReferredBy = request.ReferredBy,
        };
        var result = await Context.Accounts.AddAsync(account, dbtransaction.Item1, dbtransaction.Item2);
        if (result.ResultOrNull() == -1) AppException = new AppException("Error creating Account Item", result.AsT1.Message);
        account.AccountID = result.ResultOrNull();
        return account;
    }

    private AppException RollbackAndReturnAppError(Tuple<IDbConnection, IDbTransaction> dbtransaction)
    {
        DbHelper.RollBackTransaction(dbtransaction.Item2);
        return ApplicationError("Error Storing Data");
    }

    private AppException ApplicationError(string message)
    {
        return new AppException("AccountRegistration", $"{message} - {AppException?.Message}");
    }

    private static string GetAccountName(AccountRegistrationCommand request)
    {

        return string.IsNullOrEmpty(request.AccountName) ? $"{request.Lastname}, {request.Firstname}" : request.AccountName;
    }

    private static string GetEmailAddress(AccountRegistrationCommand request)
    {
        return request.Username.StartsWith("07") ? request.Username : "";
    }

    private int GetDefaultProfileID(AccountType accountType)
    { 
        if (registrationOptions is null) return accountType switch
        {
            AccountType.Dealer => 1,
            AccountType.Corporate => 11,
            AccountType.Selftopup => 6,
            AccountType.Retailer => 21,
            AccountType.Aggregator => 21,
            _ => 11,
        };
        return accountType switch
        {
            AccountType.Dealer => registrationOptions.DefaultDealer,
            AccountType.Corporate => registrationOptions.DefaultCompany,
            AccountType.Selftopup => registrationOptions.DefaultSelfTopup,
            AccountType.Retailer => registrationOptions.DeafultRetailer,
            AccountType.Aggregator => registrationOptions.DefaultAggregator,
            _ => 1,
        }; 
    }

    private static byte GetChannelId(AccountType accountType)
    {
        return accountType switch
        {
            AccountType.Dealer => 1,
            AccountType.Selftopup => 1,
            _ => 2,
        };
    }
}

