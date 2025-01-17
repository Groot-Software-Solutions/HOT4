using Hot4.DataModel.Models;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Abstract
{
    public interface IBundleService
    {
        Task<BundleModel?> GetBundlesById(int bundleId);
        Task<List<BundleModel>> ListBundles();
        Task<bool> AddBundle(BundleModel bundleModel);
        Task<bool> UpdateBundle(BundleModel bundleModel);
        Task<bool> DeleteBundle(BundleModel bundleModel);
    }
}
