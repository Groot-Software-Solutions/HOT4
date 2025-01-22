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
    public class ProfileDiscountService : IProfileDiscountService
    {
        private readonly IProfileDiscountRepository _profileDiscountRepository;
        private readonly IMapper Mapper;
        public ProfileDiscountService(IProfileDiscountRepository profileDiscountRepository , IMapper mapper)
        {
            _profileDiscountRepository = profileDiscountRepository;
            Mapper = mapper;
        }
        public async Task<bool> AddPrfDiscount(ProfileDiscountModel profileDiscountModel)
        {
            if (profileDiscountModel != null )
            {
               var model = Mapper.Map<ProfileDiscount>(profileDiscountModel);
                return await _profileDiscountRepository.AddPrfDiscount(model);
            }
            return false;
        }
        public async Task<bool> DeletePrfDiscount(int profileDiscountId)
        {
            var record = await GetEntityById(profileDiscountId);
            if (record != null)
            {
                return await _profileDiscountRepository.DeletePrfDiscount(record);
            }
            return false;
        }
        public async Task<ProfileDiscountModel?> GetPrfDiscountById(int profileDiscountId)
        {
            var record = await GetEntityById(profileDiscountId);
            return Mapper.Map<ProfileDiscountModel?>(record);
        }
        public async Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileAndBrandId(int profileId, int brandId)
        {
            var records = await _profileDiscountRepository.GetPrfDiscountByProfileAndBrandId(profileId , brandId);
            return  Mapper.Map<List<ProfileDiscountModel>>(records);
        }
        public async Task<List<ProfileDiscountModel>> GetPrfDiscountByProfileId(int profileId)
        {
            var records = await GetEntityById(profileId);
            return Mapper.Map<List<ProfileDiscountModel>>(records);
        }
        public async Task<bool> UpdatePrfDiscount(ProfileDiscountModel profileDiscountModel)
        {
            var record = await GetEntityById(profileDiscountModel.ProfileId);
            if (record != null)
            {
                Mapper.Map(profileDiscountModel , record);
                return await _profileDiscountRepository.UpdatePrfDiscount(record);
            }
            return false;
        }
        private async Task<ProfileDiscount?> GetEntityById (int profileDiscountId)
        {
            return await _profileDiscountRepository.GetPrfDiscountById(profileDiscountId);
        }
    }
}
