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
    public class ProductFieldService : IProductFieldService
    {
        private readonly IProductFieldRepository _productFieldRepository;
        private readonly IMapper _mapper;
        public ProductFieldService(IProductFieldRepository productFieldRepository , IMapper mapper)
        {
            _productFieldRepository = productFieldRepository;
            _mapper = mapper;
        }
        public async Task<bool> AddProductField(ProductFieldModel productFieldModel)
        {
            if (productFieldModel != null)
            {
                var model = _mapper.Map<ProductField>( productFieldModel);
               return await _productFieldRepository.AddProductField(model);
            }
            return false;
        }

        public async Task<bool> DeleteProductField(ProductFieldModel productFieldModel)
        {
            var record = await GetEntityById(productFieldModel.BrandFieldId);
            if (record != null)
            {
                return await _productFieldRepository.DeleteProductField(record);
            }
            return false;
        }

        public async Task<ProductFieldModel> GetProductFieldById(int BrandFieldId)    
        {
            var record =  await GetEntityById(BrandFieldId);
            return _mapper.Map<ProductFieldModel>(record);
        }

        public async Task<List<ProductFieldModel>> ListProductField()
        {
            var records = await _productFieldRepository.ListProductField();
            return _mapper.Map<List<ProductFieldModel>>(records);
        }

        public async Task<bool> UpdateProductField(ProductFieldModel productFieldModel)
        {
            var record = await GetEntityById(productFieldModel.BrandFieldId);
            if (record != null)
            {
                _mapper.Map(productFieldModel, record);
                return await _productFieldRepository.UpdateProductField(record);
            }
            return false;
        }

        private async Task<ProductField> GetEntityById (int BrandFieldId)
        {
            return await _productFieldRepository.GetProductFieldById(BrandFieldId);
        }
    }
}
