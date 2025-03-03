using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Nyaradzo;
using Hot.Application.RechargeServices.RechargeDataQueryHandlers;
using Hot.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.Infrastructure.RechargeServices.RechargeUtilityQueryHandlers;

public class NyaradzoQueryHandler : IRechargeUtilityQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.Nyaradzo;
    private readonly INyaradzoRechargeAPIService apiService;
    private readonly ILogger<NyaradzoQueryHandler> logger;

    public NyaradzoQueryHandler(IServiceProvider serviceProvider)
    {
        apiService = serviceProvider.GetRequiredService<INyaradzoRechargeAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<NyaradzoQueryHandler>>();
    }
    public async Task<OneOf<UtilityAccountDetailsModel, NotFoundException, NetworkProviderException, AppException>> AccountDetails(string AccountNumber)
    {
        try
        {
            if (string.IsNullOrEmpty(AccountNumber)) return new NotFoundException("AccountNumber Not Provided", "");
            var response = await apiService.QueryAccount(AccountNumber);
            if (response.Successful) return MapToAccountModel(response);
            return new NotFoundException("AccountNumber Not Found", AccountNumber);
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    private UtilityAccountDetailsModel MapToAccountModel(NyaradzoResult response)
    {
        var account = response.Account ?? throw new NetworkProviderException("Error processing data", "");
        var validBalance = account.Balance.TryParse(out decimal balance);
        var validArrears = account.Balance.TryParse(out decimal arrears);
        return new()
        {
            AccountNumber = account.PolicyNumber,
            CustomerName = account.PolicyHolder,
            Balance = validBalance ? balance : 0,
            Arears = validArrears ? arrears : 0,
            Status = account.Status,
            //Address = account.ResponseDescription,
            Currency = account.ResponseDescription.Contains("currency is USD") ? Currency.USD : Currency.ZWG,
            Message = account.ResponseDescription,
        };
    }
}
