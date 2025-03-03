using Hot.Application.Common.Extensions;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Application.Common.Models;
using Hot.Application.Common.Models.RechargeServiceModels.Telone;
using Hot.Domain.Enums;
using Hot.Infrastructure.RechargeServices.RechargeDataQueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Hot.Application.RechargeServices.RechargeDataQueryHandlers;

public class TeloneDataQueryHandler : BaseDataQueryHandler, IRechargeDataQueryHandler
{
    public int NetworkId { get; set; } = (int)Networks.Telone;

    private readonly ITeloneDataAPIService dataAPIService;
    private readonly ILogger<TeloneDataQueryHandler> logger;

    public TeloneDataQueryHandler(IServiceProvider serviceProvider)
    {
        dataAPIService = serviceProvider.GetRequiredService<ITeloneDataAPIService>();
        logger = serviceProvider.GetRequiredService<ILogger<TeloneDataQueryHandler>>();
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
        List<TeloneBundleProduct> list = new();
        var response = await dataAPIService.QueryBundles(Currency.ZWG);
        if (!response.Successful) throw new NetworkProviderException("Unable to query bundle data", "");
        list = response.BundleProducts ?? new();
        response = await dataAPIService.QueryBundles(Currency.USD);
        if (!response.Successful) throw new NetworkProviderException("Unable to query bundle data", "");
        list.AddRange(response.BundleProducts ?? new());
        BundleModels = list.SetBundleProducts(MapToBundleModel);
    }

    private static BundleModel MapToBundleModel(TeloneBundleProduct b)
    {
        return new BundleModel()
        {
            Amount = (int)(b.Amount * 100),
            BrandID = (int)b.BrandId,
            BundleID = b.ProductId,
            Description = b.Description,
            Enabled = true,
            Name = b.Name,
            Network = "Telone",
            ProductCode = b.ProductId.ToString(),
            ValidityPeriod = 30,
        };
    }


}