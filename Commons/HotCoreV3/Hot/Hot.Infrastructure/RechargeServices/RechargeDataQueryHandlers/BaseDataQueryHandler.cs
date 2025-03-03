using Hot.Application.Common.Models;

namespace Hot.Infrastructure.RechargeServices.RechargeDataQueryHandlers
{
    public class BaseDataQueryHandler
    {
        internal Dictionary<string, BundleModel>? BundleModels { get; set; }
        internal List<BundleModel> GetList() => (BundleModels ?? new()).Select(b => b.Value).ToList();

        
    }
}
