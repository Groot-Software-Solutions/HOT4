using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Domain.Enums;
using Hot.Infrastructure.RechargeServices.RechargeDataQueryHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OneOf;

namespace Hot.Application.RechargeServices.RechargeDataQueryHandlers;
public class EconetDataQueryHandler : BaseDataQueryHandler, IRechargeDataQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.Econet;

    private readonly IEconetRechargeDataAPIService dataAPIService;
    private readonly ILogger<EconetDataQueryHandler> logger;

    public EconetDataQueryHandler(IServiceProvider serviceProvider)
    {
        dataAPIService = serviceProvider.GetRequiredService<IEconetRechargeDataAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<EconetDataQueryHandler>>();
    }

    public async Task<OneOf<BundleModel?, NetworkProviderException, AppException>> GetBundle(string ProductCode)
    {
        try
        {
            if (BundleModels is not null) return BundleModels[ProductCode];
            await LoadBundlesAsync();
            return BundleModels?[ProductCode];
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    public async Task<OneOf<List<BundleModel>, NetworkProviderException, AppException>> GetBundles()
    {
        try
        {
            if (BundleModels is not null) return GetList();
            await LoadBundlesAsync();
            return GetList();
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }

    public async Task<OneOf<string, NotFoundException, NetworkProviderException, AppException>> GetName(string ProductCode)
    {
        try
        {
            if (string.IsNullOrEmpty(ProductCode)) return new NotFoundException("Product Code Not Provided", "");
            if (BundleModels is null) await LoadBundlesAsync();
            var list = BundleModels ?? new();
            if (!list.ContainsKey(ProductCode)) return new NotFoundException("Product Code not found", ProductCode);
            return list[ProductCode].Name;
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }

    }
    public async Task<OneOf<bool, NetworkProviderException, AppException>> IsValidProduct(string ProductCode)
    {
        try
        {
            if (string.IsNullOrEmpty(ProductCode)) return false;
            if (BundleModels is null) await LoadBundlesAsync();
            var list = BundleModels ?? new();
            return list.ContainsKey(ProductCode);
        }
        catch (NetworkProviderException ex) { return ex; }
        catch (Exception ex) { return ex.LogAndReturnError(logger); }
    }

    private async Task LoadBundlesAsync()
    {
        var response = await dataAPIService.QueryBundles();
        if (!response.Successful) throw new NetworkProviderException("Unable to query bundle data", "");
        BundleModels = response.BundleProducts.SetBundleProducts(MapToBundleModel);
    }

    private static BundleModel MapToBundleModel(EconetBundleProduct b)
    {
        return new BundleModel()
        {
            Amount = b.Amount,
            BrandID = b.BrandID,
            BundleID = b.BundleID,
            Description = b.Description,
            Enabled = b.Enabled,
            Name = b.Name,
            Network = b.Network,
            ProductCode = b.ProductCode,
            ValidityPeriod = b.ValidityPeriod,
        };
    }

     
}

public class Econet078DataQueryHandler : EconetDataQueryHandler, IRechargeDataQueryHandler
{
    public Econet078DataQueryHandler(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public new int NetworkId { get; set; } = (int)Networks.Econet078;


}
