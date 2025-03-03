using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;

namespace Hot.Infrastructure.RechargeServices.RechargeMobileQueryHandler;

public class EconetMobileQueryHandler : IRechargeMobileQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.Econet;
    private readonly IEconetRechargeAPIService apiService;
    private readonly ILogger<EconetMobileQueryHandler> logger;

    public EconetMobileQueryHandler(IServiceProvider serviceProvider)
    {
        apiService = serviceProvider.GetRequiredService<IEconetRechargeAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<EconetMobileQueryHandler>>();
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

    private MobileAccountDetailsModel MapToAccountModel(string AccountNumber, EconetBalanceResult response)
    {
        return new()
        {
            AccountNumber = AccountNumber,
            CustomerName = AccountNumber,
            Balance = response.Balance,
            Arears = 0,
            Status = "Active",
        };
    }
}
public class Econet078MobileQueryHandler : EconetMobileQueryHandler, IRechargeMobileQueryHandler
{
    public new int NetworkId { get; set; } = (int)Networks.Econet078;

    public Econet078MobileQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}
