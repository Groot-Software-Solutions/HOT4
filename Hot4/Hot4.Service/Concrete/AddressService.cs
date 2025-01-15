using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;

namespace Hot4.Service.Concrete
{
    public class AddressService : IAddressService
    {

        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<AddressModel?> GetAddressById(long accountId)
        {
            var result = await _addressRepository.GetAddressById(accountId);
            return _mapper.Map<AddressModel>(result);   
        }

        public async Task<bool> SaveAddress(AddressModel addressModel)
        {
            var Payload =  _mapper.Map<Address>(addressModel);
            var result = _addressRepository.SaveAddress(Payload);
            return true;
            
        }

        public async Task<bool> UpdateAddress(AddressModel addressModel)
        {
            var Payload = _mapper.Map<Address>(addressModel);
            var result = _addressRepository.UpdateAddress(Payload);
            return true;
        }

        public Task DeleteAddress(AddressModel addressModel)
        {
            var Payload = _mapper.Map<Address>(addressModel);
            var result = _addressRepository.UpdateAddress(Payload);
            return result;
        }
    }
}
