using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;
public static class AddressDbFacker
{
 
    //public static IDbContext Address(this DbFakerService service, Address address)
    //{
    //    service.dbContext.Accounts.GetAsync(Arg.Any<int>()).Returns(address);
    //    return service.dbContext;
    //}

    public static IDbContext AddressUpdate(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.Addresses.Update(Arg.Any<Address>()).Returns(expectedResult);
        service.dbContext.Addresses.UpdateAsync(Arg.Any<Address>()).Returns(expectedResult);
        return service.dbContext;
    }
    //public static IDbContext GetAddress(this DbFakerService service, Address address)
    //{

    //    service.dbContext.Addresses.GetAsync(Arg.Any<long>()).Returns(GetSingle(address));
    //    return service.dbContext;
    //}

    public static IDbContext GetAddress(this DbFakerService service, Address address)
    {
        service.dbContext.Addresses.Get(Arg.Any<long>()).Returns(address);
        service.dbContext.Addresses.GetAsync(Arg.Any<long>()).Returns(address);
        return service.dbContext;
    }



    public static Address GetSingle()
    {  
             return GetAddressFaker().Generate();
    }
    public static Faker<Address> GetAddressFaker()
    {
        var addressFaker = new Faker<Address>()
             .RuleFor(a => a.AccountID, f => f.Random.Long(100000, 100000000))
             .RuleFor(a => a.Address1, f => f.Address.StreetAddress())
             .RuleFor(a => a.Address2, f => f.Address.SecondaryAddress())
             .RuleFor(a => a.City, f => f.Address.City())
             .RuleFor(a => a.ContactName, f => f.Name.FullName())
             .RuleFor(a => a.ContactNumber, f => f.Phone.PhoneNumber())
             .RuleFor(a => a.VatNumber, f => f.Random.AlphaNumeric(10))
             .RuleFor(a => a.Latitude, f => f.Address.Latitude())
             .RuleFor(a => a.Longitude, f => f.Address.Longitude())
             .RuleFor(a => a.SageID, f => f.Random.Long(300000000, 600000000))
             .RuleFor(a => a.InvoiceFreq, f => f.Random.Byte(0, 1));
        return addressFaker;
    }

}