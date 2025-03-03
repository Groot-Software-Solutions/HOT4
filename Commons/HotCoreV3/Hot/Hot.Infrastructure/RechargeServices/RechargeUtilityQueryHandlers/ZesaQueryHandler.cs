using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.ZESA;
using Hot.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.Infrastructure.RechargeServices.RechargeUtilityQueryHandlers;

public class ZesaQueryHandler : IRechargeUtilityQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.ZESA;
    private readonly IZESARechargeAPIService apiService;
    private readonly ILogger<ZesaQueryHandler> logger;

    public ZesaQueryHandler(IServiceProvider serviceProvider)
    {
        apiService = serviceProvider.GetRequiredService<IZESARechargeAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<ZesaQueryHandler>>();
    }

    public async Task<OneOf<UtilityAccountDetailsModel, NotFoundException, NetworkProviderException, AppException>> AccountDetails(string AccountNumber)
    {
        try
        {
            if (string.IsNullOrEmpty(AccountNumber)) return new NotFoundException("Meter Number Not Provided", "");
            var response = await apiService.QueryZESAAccount(AccountNumber);
            if (response.Successful) return MapToAccountModel(response);
            return new NotFoundException("Meter Number Not Found", AccountNumber);
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    private UtilityAccountDetailsModel MapToAccountModel(ZESAAccountQueryResult response)
    {
        var account = response.CustomerInfo ?? throw new NetworkProviderException("Error processing data", "");
        var validCurrency = Enum.TryParse(account.Currency, out Currency currency); 
        return new()
        {
            AccountNumber = account.MeterNumber,
            CustomerName = account.CustomerName,
            Balance = 0,
            Arears = 0,
            Status = "Active",
            Address = account.Address,
            Currency = validCurrency ? currency : Currency.ZWG,
        };
    }
}
