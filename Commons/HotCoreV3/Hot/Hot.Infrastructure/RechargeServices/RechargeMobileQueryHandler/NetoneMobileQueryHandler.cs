using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;

namespace Hot.Infrastructure.RechargeServices.RechargeMobileQueryHandler;

public class NetoneMobileQueryHandler : IRechargeMobileQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.NetOne;
    private readonly INetOneRechargeAPIService apiService;
    private readonly ILogger<NetoneMobileQueryHandler> logger;

    public NetoneMobileQueryHandler(IServiceProvider serviceProvider)
    {
        apiService = serviceProvider.GetRequiredService<INetOneRechargeAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<NetoneMobileQueryHandler>>();
    }

    public async Task<OneOf<MobileAccountDetailsModel, NotFoundException, NetworkProviderException, AppException>> AccountDetails(string AccountNumber)
    {
        try
        {
            if (string.IsNullOrEmpty(AccountNumber)) return new NotFoundException("Meter Number Not Provided", "");
            var response = await apiService.QueryEndUserBalance(AccountNumber);
            if (response.Successful) return MapToAccountModel(AccountNumber, response);
            return new NotFoundException("Mobile Number Not Found", AccountNumber);
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    private MobileAccountDetailsModel MapToAccountModel(string AccountNumber, NetOneRechargeResult response)
    {
        return new()
        {
            AccountNumber = AccountNumber,
            CustomerName = AccountNumber,
            Balance = response.FinalBalance,
            Arears = 0,
            Status = "Active",
        };
    }
}