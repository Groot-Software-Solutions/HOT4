using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Telone;
using Hot.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.Infrastructure.RechargeServices.RechargeUtilityQueryHandlers;

public class TeloneQueryHandler : IRechargeUtilityQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.Telone;
    private readonly ITeloneDataAPIService apiService;
    private readonly ILogger<TeloneQueryHandler> logger;

    public TeloneQueryHandler(IServiceProvider serviceProvider)
    {
        apiService = serviceProvider.GetRequiredService<ITeloneDataAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<TeloneQueryHandler>>();
    }
    public async Task<OneOf<UtilityAccountDetailsModel, NotFoundException, NetworkProviderException, AppException>> AccountDetails(string AccountNumber)
    {
        try
        {
            if (string.IsNullOrEmpty(AccountNumber)) return new NotFoundException("AccountNumber Not Provided");
            var response = await apiService.QueryAccount(AccountNumber);
            if (response.Successful) return MapToAccountModel(response);
            return new NotFoundException(response.TransactionResult);
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    private static UtilityAccountDetailsModel MapToAccountModel(TeloneCustomerResult response)
    {
        var account = response.Account ?? throw new NetworkProviderException("Error processing data", "");
        
        return new()
        {
            AccountNumber = account.AccountNumber,
            CustomerName = account.AccountName, 
            Status = "Active",
            Address = account.AccountAddress, 
            Message = account.ResponseDescription,
            Currency = null,
        };
    }
}