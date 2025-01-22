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
    public class ProductMetaDataTypeService : IProductMetaDataTypeService
    {
        private readonly IProductMetaDataTypeRepository _productMetaDataTypeRepository;
        private readonly IMapper Mapper;

        public ProductMetaDataTypeService(IProductMetaDataTypeRepository productMetaDataTypeRepository, IMapper mapper)
        {
            _productMetaDataTypeRepository = productMetaDataTypeRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddProductMetaDataType(ProductMetaDataTypeModel productMetaDataTypeModel)
        {
            if (productMetaDataTypeModel != null)
            {
              var model =  Mapper.Map<ProductMetaDataType>(productMetaDataTypeModel);
                return await _productMetaDataTypeRepository.AddProductMetaDataType(model);
            }
            return false;
        }
        public async Task<bool> DeleteProductMetaDataType(byte productMetaDataTypeId)
        {
            var record = await GetEntityById(productMetaDataTypeId);
            if (record != null)
            {
               return await _productMetaDataTypeRepository.DeleteProductMetaDataType(record);
            }
            return false;
        }
        public async Task<ProductMetaDataTypeModel> GetProductMetaDataTypeById(byte productMetaDataTypeId)
        {
            var record = await _productMetaDataTypeRepository.GetProductMetaDataTypeById(productMetaDataTypeId);
            return Mapper.Map<ProductMetaDataTypeModel>(record);   
        }
        public async Task<List<ProductMetaDataTypeModel>> ListProductMetaDataType()
        {
            var records = await _productMetaDataTypeRepository.ListProductMetaDataType();
            return  Mapper.Map<List<ProductMetaDataTypeModel>>(records);
        }
        public async Task<bool> UpdateProductMetaDataType(ProductMetaDataTypeModel productMetaDataTypeModel)
        {
            var record = await GetEntityById(productMetaDataTypeModel.ProductMetaDataTypeId); 
            if (record != null)
            {
                Mapper.Map(productMetaDataTypeModel, record);
              await _productMetaDataTypeRepository.UpdateProductMetaDataType(record);
            }
            return false;
        }
        private async Task<ProductMetaDataType> GetEntityById (byte productMetaDataTypeId)
        {
            return await _productMetaDataTypeRepository.GetProductMetaDataTypeById (productMetaDataTypeId);
        }
    }
}
