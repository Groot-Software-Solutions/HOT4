using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using Microsoft.Identity.Client;
using System.Net;

namespace Hot4.Service.Concrete
{
    public class AddressService : IAddressService
    {

        private readonly IAddressRepository _addressRepository;
        private readonly IMapper Mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            Mapper = mapper;
        }
        public async Task<AddressModel?> GetAddressById(long accountId)
        {
            var record = await GetEntityById(accountId);
            return Mapper.Map<AddressModel>(record);
        }
        public async Task<bool> SaveAddress(AddressModel addressModel)
        {
            if (addressModel != null)
            {
                var model = Mapper.Map<Address>(addressModel);
               return await _addressRepository.SaveAddress(model);
            }
            return false;
            
        }
        public async Task<bool> UpdateAddress(AddressModel addressModel)
        {
            var record = await GetEntityById(addressModel.AccountId);
            if (record != null)
            {
                Mapper.Map(addressModel, record);
                return await _addressRepository.UpdateAddress(record);
            }
            return false;
        }
        public async Task<bool> DeleteAddress(long AccountId)
        {
            var record = await GetEntityById(AccountId); 
            if (record != null )
            {
               return await _addressRepository.DeleteAddress(record);
            }
            return false;            
        }
        private async Task<Address?> GetEntityById(long AccountId)
        {
            return await _addressRepository.GetAddressById(AccountId);
        }
    }
}
