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
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper Mapper;
        public ProductService(IProductRepository productRepository , IMapper mapper)
        {
            _productRepository = productRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddProduct(ProductModel productModel)
        {
            if (productModel != null) 
            {
                var model = Mapper.Map<Product>(productModel);
                return await _productRepository.AddProduct(model);
            }
            return false;
        }
        public async Task<bool> DeleteProduct(byte productId)
        {
            var record = await GetEntityById(productId);
            if (record != null)
            {
                return await _productRepository.DeleteProduct(record);
            }
            return false;
        }
        public async Task<ProductModel?> GetProductById(byte productId)
        {
            var record =  await GetEntityById(productId);
            return Mapper.Map<ProductModel?>(record);
        }
        public async Task<List<ProductModel>> ListProduct()
        {
            var records = await _productRepository.ListProduct();
            return Mapper.Map<List<ProductModel>>(records);
        }
        public async Task<bool> UpdateProduct(ProductModel productModel)
        {
            var record = await GetEntityById(productModel.ProductId);
            if (record != null)
            {
                Mapper.Map(productModel, record);
                await _productRepository.UpdateProduct(record);
            }
            return false;
        }
        private async Task<Product?> GetEntityById (byte ProductId)
        {
            return await _productRepository.GetProductById(ProductId);
        } 
    }
}
