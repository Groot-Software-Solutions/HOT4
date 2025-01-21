using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper Mapper;
        public BrandService(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddBrand(BrandToDo brand)
        {
            if (brand != null)
            {
                var model = Mapper.Map<Brand>(brand);
                return await _brandRepository.AddBrand(model);
            }
            return false;           
        }
        public async Task<bool> DeleteBrand(byte brandId)
        {
            var record = await GetEntityById(brandId);
            if (record != null)
            {
                return await _brandRepository.DeleteBrand(record);
            }
            return false;
        }
        public async Task<BrandModel?> GetBrandById(byte brandId)
        {
            var record = await GetEntityById(brandId);
            return Mapper.Map<BrandModel>(record);
        }
        public async Task<List<BrandModel>> GetBrandIdentity(BrandIdentitySearchModel brandIdentitySearchModel)
        {
            var records = await _brandRepository.GetBrandIdentity(brandIdentitySearchModel);
            return Mapper.Map<List<BrandModel>>(records);
        }
        public async Task<List<BrandModel>> ListBrand()
        {
            var records = await _brandRepository.ListBrand();
            return Mapper.Map<List<BrandModel>>(records);
        }
        public async Task<bool> UpdateBrand(BrandToDo brand)
        {
            var record = await GetEntityById(brand.BrandId);
            if (record != null)
            {
                Mapper.Map(brand, record);
                return await _brandRepository.UpdateBrand(record);
            }
            return false;
        }
        private async Task<Brand?> GetEntityById (byte BrandId)
        {
            return await _brandRepository.GetBrandById(BrandId);
        }
    }
}
