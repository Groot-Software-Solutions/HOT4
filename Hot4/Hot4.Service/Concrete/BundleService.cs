using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BundleService : IBundleService
    {
        private readonly IBundleRepository _bundleRepository;
        private readonly IMapper _mapper;

        public BundleService(IBundleRepository bundleRepository, IMapper mapper)
        {
            _bundleRepository = bundleRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddBundle(BundleToDo bundleModel)
        {
            if (bundleModel != null)
            {
                var model = _mapper.Map<Bundle>(bundleModel);
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
            return _mapper.Map<BundleModel?>(record);
        }
        public async Task<List<BundleModel>> ListBundles()
        {
            var records = await _bundleRepository.ListBundles();
            return _mapper.Map<List<BundleModel>>(records);

        }
        public async Task<bool> UpdateBundle(BundleToDo bundleModel)
        {
            var record = await GetEntityById(bundleModel.BundleId);

            if (record != null)
            {
                var model = _mapper.Map(bundleModel, record);
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
