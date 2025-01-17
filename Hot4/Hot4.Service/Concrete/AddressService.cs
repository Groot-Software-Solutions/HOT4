using AutoMapper;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.Service.Abstract;
using Hot4.ViewModel;
using System.Net;

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
            var record = await _addressRepository.GetAddressById(accountId);
            if (record != null)
            {
                return new AddressModel
                {
                    AccountId = record.AccountId,
                    Address1 = record.Address1 ?? "",
                    Address2 = record.Address2 ?? "",
                    City = record.City ?? "",
                    ContactName = record.ContactName ?? "",
                    ContactNumber = record.ContactNumber ?? "",
                    VatNumber = record.VatNumber ?? "",
                    Latitude = record.Latitude ?? 0,
                    Longitude = record.Longitude ?? 0,
                    SageId = record.SageId ?? 0,
                    SageIdusd = record.SageIdusd ?? 0,
                    InvoiceFreq = record.InvoiceFreq ?? 0,
                    Confirmed = record.Confirmed ?? false
                };
            }
            else
            {
                return new AddressModel
                {
                    AccountId = accountId,
                    Address1 = "",
                    Address2 = "",
                   City = "",
                    ContactName = "",
                    ContactNumber = "",
                    VatNumber = "",
                    Latitude = 0,
                    Longitude = 0,
                    SageId = 0,
                    SageIdusd = 0,
                    InvoiceFreq = 0,
                    Confirmed = false
                };
            }
        }
        public async Task<bool> SaveAddress(AddressModel addressModel)
        {
            if (addressModel != null)
            {
                var model = _mapper.Map<Address>(addressModel);
               return await _addressRepository.SaveAddress(model);
            }
            return false;
            
        }
        public async Task<bool> UpdateAddress(AddressModel addressModel)
        {
            var record = await _addressRepository.GetAddressById(addressModel.AccountId);
            if (record != null)
            {
                _mapper.Map(addressModel, record);
                return await _addressRepository.UpdateAddress(record);
            }
            return false;
        }
        public async Task<bool> DeleteAddress(AddressModel addressModel)
        {
            var record = await _addressRepository.GetAddressById(addressModel.AccountId); 
            if (record != null )
            {
               return await _addressRepository.DeleteAddress(record);
            }
            return false;
            
        }
    }
}
