﻿using Hot4.DataModel.Data;
using Hot4.DataModel.Models;
using Hot4.Repository.Abstract;
using Hot4.ViewModel;

namespace Hot4.Repository.Concrete
{
    public class AddressRepository : RepositoryBase<Address>, IAddressRepository
    {
        public AddressRepository(HotDbContext context) : base(context) { }

        //public async Task<AddressModel?> GetAddressById(long accountId)
        //{
        //    var address = await GetById(accountId);
        //    if (address != null)
        //    {
        //        return new AddressModel
        //        {
        //            AccountId = address.AccountId,
        //            Address1 = address.Address1 ?? "",
        //            Address2 = address.Address2 ?? "",
        //            City = address.City ?? "",
        //            ContactName = address.ContactName ?? "",
        //            ContactNumber = address.ContactNumber ?? "",
        //            VatNumber = address.VatNumber ?? "",
        //            Latitude = address.Latitude ?? 0,
        //            Longitude = address.Longitude ?? 0,
        //            SageId = address.SageId ?? 0,
        //            SageIdusd = address.SageIdusd ?? 0,
        //            InvoiceFreq = address.InvoiceFreq ?? 0,
        //            Confirmed = address.Confirmed ?? false
        //        };
        //    }
        //    else
        //    {
        //        return new AddressModel
        //        {
        //            AccountId = accountId,
        //            Address1 = "",
        //            Address2 = "",
        //            City = "",
        //            ContactName = "",
        //            ContactNumber = "",
        //            VatNumber = "",
        //            Latitude = 0,
        //            Longitude = 0,
        //            SageId = 0,
        //            SageIdusd = 0,
        //            InvoiceFreq = 0,
        //            Confirmed = false
        //        };
        //    }
        //}



        public async Task<Address?> GetAddressById(long accountId)
        {
            var address = await GetById(accountId);
            if (address != null)
            {
                return address;
            }

            return null;
        }



        public async Task SaveAddress(Address address)
        {
            address.InvoiceFreq = 1;
            await Create(address);
            await SaveChanges();
        }
        public async Task UpdateAddress(Address address)
        {
            Update(address);
            await SaveChanges();
        }
        public async Task DeleteAddress(Address address)
        {
            Delete(address);
            await SaveChanges();
        }
    }
}
