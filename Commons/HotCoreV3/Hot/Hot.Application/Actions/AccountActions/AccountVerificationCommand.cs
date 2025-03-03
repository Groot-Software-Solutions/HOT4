using Hot.Application.Common.Models.Options;
using Microsoft.Extensions.Configuration;
using System.Data;


namespace Hot.Application.Actions.AccountActions;
public record AccountVerificationCommand
    (long AccountId, string Firstname, string Lastname,
        string IDNumber, string Address, string City, string Email, string Sender)
    : IRequest<OneOf<bool, AppException>>;

public class AccountVerificationCommandHandler : IRequestHandler<AccountVerificationCommand, OneOf<bool, AppException>>
{
    private IDbContext Context { get; set; }
    private IDbHelper DbHelper { get; set; }
    private AppException? AppException { get; set; }

    public AccountVerificationCommandHandler(IDbContext context, IDbHelper dbHelper, IConfiguration configuration)
    {
        Context = context;
        DbHelper = dbHelper;
    }

    public async Task<OneOf<bool, AppException>> Handle(AccountVerificationCommand request, CancellationToken cancellationToken)
    {

        //if (!IsAllowedToRegisterAccounts(request.Sender)) return ApplicationError("Unauthorised");
        var accountResult = await Context.Accounts.GetAsync(request.AccountId);
        if (accountResult.IsT1) return new AppException("Failed to get the account", accountResult.AsT1.Message);
        var account = accountResult.AsT0;

        var dbtransaction = await BeginTransaction();
        if (dbtransaction == null) return ApplicationError("Data Connection error");
        //Update Account
        account.AccountName = $"{request.Firstname},{request.Lastname}";
        account.NationalID = request.IDNumber;
        account.Email = request.Email;

        var accountUpdateResult = await Context.Accounts.UpdateAsync(account);
        if (accountUpdateResult.IsT1) return RollbackAndReturnAppError(dbtransaction);

        //Address
        var AddressResult = await Context.Addresses.GetAsync(request.AccountId);
        if (AddressResult.IsT1)
        {
            if (!AddressResult.AsT1.IsNotFoundException()) return RollbackAndReturnAppError(dbtransaction);

            Address NewAddress = new()
            {
                AccountID = request.AccountId
            ,
                Address1 = request.Address
            ,
                City = request.City
            ,
                Confirmed = true
            };
            var AddAddressResult = await Context.Addresses.AddAsync(NewAddress);
            if (AddAddressResult.IsT1) return RollbackAndReturnAppError(dbtransaction);

            bool addResult = CompleteTransaction(dbtransaction);
            if (addResult == false) return RollbackAndReturnAppError(dbtransaction);

            return true;
        }

        var address = AddressResult.AsT0;
        address.Address1 = request.Address;
        address.City = request.City;
        address.ContactName = $" {request.Firstname} {request.Lastname}";
        address.Confirmed = true;

        var AddressUpdateResponse = await Context.Addresses.UpdateAsync(address);
        if (AddressUpdateResponse.IsT1) return RollbackAndReturnAppError(dbtransaction);

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


    private AppException RollbackAndReturnAppError(Tuple<IDbConnection, IDbTransaction> dbtransaction)
    {
        DbHelper.RollBackTransaction(dbtransaction.Item2);
        return ApplicationError("Error Storing Data");
    }

    private AppException ApplicationError(string message)
    {
        return new AppException("AccountVerification", $"{message} - {AppException?.Message}");
    }

}
