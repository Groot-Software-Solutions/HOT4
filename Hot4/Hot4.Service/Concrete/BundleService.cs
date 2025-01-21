using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BundleService : IBundleService
    {
        private readonly IBundleRepository  _bundleRepository;
        private readonly IMapper Mapper;
        public BundleService(IBundleRepository bundleRepository , IMapper mapper)
        {
            _bundleRepository = bundleRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddBundle(BundleRecord bundleModel)
        {
            if (bundleModel != null)
            {
             var model = Mapper.Map<Bundle>(bundleModel);
            return await _bundleRepository.AddBundle(model);
            }
            return false;
        }    
        public async Task<bool> DeleteBundle(int bundleId)
        {
            var record = await GetEntityById(bundleId);
            if (record != null)
            {
                return await _bundleRepository.DeleteBundle(record);
            }
            return false;
        }

        public async Task<BundleModel?> GetBundlesById(int bundleId)
        {
            var record = await GetEntityById(bundleId);
            return Mapper.Map<BundleModel?>(record);
        }
        public async Task<List<BundleModel>> ListBundles()
        {
            var records = await _bundleRepository.ListBundles();
            return Mapper.Map<List<BundleModel>>(records);

        }
        public async Task<bool> UpdateBundle(BundleRecord bundleModel)
        {
            var record = await GetEntityById(bundleModel.BundleId);

            if (record != null)
            {
                var model = Mapper.Map(bundleModel, record);
                return await _bundleRepository.UpdateBundle(record);

            }
            return false;
        }

        private async Task<Bundle?> GetEntityById(int bundleId)
        {
            return await _bundleRepository.GetBundlesById(bundleId);
        }
    }
}
