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
    public class ProductMetaDataService : IProductMetaDataService
    {
        private readonly IProductMetaDataRepository _productMetaDataRepository;
        private readonly IMapper Mapper;
        public ProductMetaDataService(IProductMetaDataRepository productMetaDataRepository , IMapper mapper)
        {
            _productMetaDataRepository = productMetaDataRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddProductMetaData(ProductMetaDataModel productMetaDataModel)
        {
            if (productMetaDataModel != null)
            {
                var model = Mapper.Map<ProductMetaData>(productMetaDataModel);
                return await _productMetaDataRepository.AddProductMetaData(model);
            }
            return false;
        }
        public async Task<bool> DeleteProductMetaData(int productMetaId)
        {
            var record = await GetEntityById(productMetaId);
            if (record != null)
            {
                return await _productMetaDataRepository.DeleteProductMetaData(record);
            }
            return false;

        }
        public async Task<ProductMetaDataModel> GetProductMetaDataById(int productMetaId)
        {
            var record = await GetEntityById(productMetaId);
            return  Mapper.Map<ProductMetaDataModel>(record);
        }
        public async Task<List<ProductMetaDataModel>> ListProductMetaData()
        {
            var records = await _productMetaDataRepository.ListProductMetaData();
            return Mapper.Map<List<ProductMetaDataModel>>(records);
        }
        public async Task<bool> UpdateProductMetaData(ProductMetaDataModel productMetaDataModel)
        {
            var record = await GetEntityById(productMetaDataModel.BrandMetaId);
            if(record != null)
            {
                Mapper.Map(productMetaDataModel, record);
               await _productMetaDataRepository.UpdateProductMetaData(record);
            }
            return false;
        }
        private async Task<ProductMetaData> GetEntityById (int productMetaId)
        {
            return await _productMetaDataRepository.GetProductMetaDataById(productMetaId);
        }
    }
}
