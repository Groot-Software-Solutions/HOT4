using System.Security.Principal;

namespace Hot.Application.Common.Interfaces;

public interface IRechargeDataQueryHandler
{
    public int NetworkId { get; set; }

    public Task<OneOf<BundleModel?, NetworkProviderException, AppException>> GetBundle(string ProductCode);
    public Task<OneOf<List<BundleModel>, NetworkProviderException, AppException>> GetBundles();
    public Task<OneOf<string, NotFoundException, NetworkProviderException, AppException>> GetName(string ProductCode);
    public Task<OneOf<bool, NetworkProviderException, AppException>> IsValidProduct(string ProductCode);

}

