using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot4.Service.Concrete
{
    public class BundleService : IBundleService
    {
        private readonly IBundleRepository  _bundleRepository;
        private readonly IMapper _mapper;

        public BundleService(IBundleRepository bundleRepository , IMapper mapper)
        {
            _bundleRepository = bundleRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddBundle(BundleModel bundleModel)
        {
            if (bundleModel != null)
            {
             var model = _mapper.Map<Bundle>(bundleModel);
            return   await _bundleRepository.AddBundle(model);
            }
            return false;
        }
       
        public async Task<bool> DeleteBundle(BundleModel bundleModel)
        {
            var record = await _bundleRepository.GetBundlesById(bundleModel.BundleId);
            if (record != null)
            {
                var model = _mapper.Map<Bundle>(record);
             return  await _bundleRepository.DeleteBundle(model);
               
            }
            return false;   
        }

        public async Task<BundleModel?> GetBundlesById(int bundleId)
        {
            return await _bundleRepository.GetBundlesById(bundleId);
        }        
        public async Task<List<BundleModel>> ListBundles()
        {
            return await _bundleRepository.ListBundles();
           
        }      
        public async Task<bool> UpdateBundle(BundleModel bundleModel)
        {
            var record = await _bundleRepository.GetBundlesById(bundleModel.BundleId);

            if (record != null) 
            {
                var model = _mapper.Map<Bundle>(record); 
              return  await _bundleRepository.UpdateBundle(model);
               
            }
            return false;
        }

        //private async Task<Bundle?> GetEntityById (int BundleId)
        //{
        //    return await _bundleRepository.GetBundlesById(BundleId);
        //}
    }
}
