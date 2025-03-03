using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.NetOne;
using Hot.Domain.Enums;
using Hot.Infrastructure.RechargeServices.RechargeDataQueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.Application.RechargeServices.RechargeDataQueryHandlers;

public class NetoneDataQueryHandler : BaseDataQueryHandler, IRechargeDataQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.NetOne;

    private readonly INetoneRechargeDataAPIService dataAPIService;
    private readonly ILogger<NetoneDataQueryHandler> logger;

    public NetoneDataQueryHandler(IServiceProvider serviceProvider)
    {
        dataAPIService = serviceProvider.GetRequiredService<INetoneRechargeDataAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<NetoneDataQueryHandler>>();
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

    private static BundleModel MapToBundleModel(NetoneBundleProduct b)
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
