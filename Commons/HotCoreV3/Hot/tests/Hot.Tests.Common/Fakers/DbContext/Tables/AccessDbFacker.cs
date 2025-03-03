using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class AccessDbFacker
{
    public static IDbContext RandomAccessList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Accesss.Search(Arg.Any<string>()).Returns(GetList(count));
        service.dbContext.Accesss.SearchAsync(Arg.Any<string>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext AccessList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Accesss.List().Returns(GetList(count));
        service.dbContext.Accesss.ListAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext AccessByAccountIdList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Accesss.List(Arg.Any<long>()).Returns(GetList(count));
        service.dbContext.Accesss.ListAsync(Arg.Any<long>()).Returns(GetList(count));
        return service.dbContext;
    }
 
    public static IDbContext RandomAccess(this DbFakerService service)
    {
        return Access(service, GetSingle());
    }
    public static IDbContext Access(this DbFakerService service, Access access)
    {
        service.dbContext.Accesss.Get(Arg.Any<int>()).Returns(access);
        service.dbContext.Accesss.GetAsync(Arg.Any<int>()).Returns(access);
        return service.dbContext;
    }
    public static IDbContext Address(this DbFakerService service, Address address)
    {
        service.dbContext.Addresses.Get(Arg.Any<int>()).Returns(address);
        service.dbContext.Addresses.GetAsync(Arg.Any<int>()).Returns(address);
        return service.dbContext;
    }
    public static IDbContext AccessPasswordChange(this DbFakerService service,bool expectedResult)
    {
        service.dbContext.Accesss.PasswordChange(Arg.Any<Access>()).Returns(expectedResult);
        service.dbContext.Accesss.PasswordChangeAsync(Arg.Any<Access>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext AccessSelectLogin(this DbFakerService service)
    {
        service.dbContext.Accesss.SelectLogin(Arg.Any<string>(), Arg.Any<string>()).Returns(GetSingle());
        service.dbContext.Accesss.SelectLoginAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(GetSingle());
        return service.dbContext;
    }
    public static IDbContext AccessSelectCode(this DbFakerService service)
    {
        service.dbContext.Accesss.SelectCode(Arg.Any<string>()).Returns(GetSingle());
        service.dbContext.Accesss.SelectCodeAsync(Arg.Any<string>()).Returns(GetSingle());
        return service.dbContext;
    }
    public static IDbContext AccessRemove(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.Accesss.Remove(Arg.Any<int>()).Returns(expectedResult);
        service.dbContext.Accesss.RemoveAsync(Arg.Any<int>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext AccessUnDelete(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.Accesss.UnDelete(Arg.Any<long>()).Returns(expectedResult);
        service.dbContext.Accesss.UnDeleteAsync(Arg.Any<long>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext AccessUpdate(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.Accesss.Update(Arg.Any<Access>()).Returns(expectedResult);
        service.dbContext.Accesss.UpdateAsync(Arg.Any<Access>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext AccessAdd(this DbFakerService service, Access access)
    {
        service.dbContext.Accesss.Add(Arg.Any<Access>()).Returns((int)access.AccessId);
        service.dbContext.Accesss.AddAsync(Arg.Any<Access>()).Returns((int)access.AccessId);
        return service.dbContext;
    }
    public static IDbContext AccessWebUpdate(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.AccessWebs.Update(Arg.Any<AccessWeb>()).Returns(expectedResult);
        service.dbContext.AccessWebs.UpdateAsync(Arg.Any<AccessWeb>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext GetAccessWeb(this DbFakerService service, AccessWeb accessWeb)
    {
        service.dbContext.AccessWebs.GetAsync(Arg.Any<int>()).Returns(accessWeb);
        return service.dbContext;
    }
    public static Access GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Access> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    public static AccessWeb GetAccessWebSingle()
    {
        return GetAccessWebFaker().Generate();
    }

    public static List<AccessWeb> GetAccessWebList(int count)
    {
        return GetAccessWebFaker().Generate(count);
    }
    public static Address GetAddressSingle()
    {
        return AddressDbFacker.GetAddressFaker().Generate();
    }
    private static Faker<Access> GetFaker()
    {
        DateTime startDate = Convert.ToDateTime("01-01-2008");
        DateTime endDate = DateTime.Now;
        var accessFaker = new Faker<Access>()
                    .RuleFor(a => a.AccessId, f => f.Random.Long(100000,1000000))
                    .RuleFor(a => a.AccessCode, f => f.Phone.PhoneNumber("##########"))
                    .RuleFor(a => a.ChannelID, f => f.Random.Byte(1,3))
                    .RuleFor(a => a.AccessPassword, f => f.Internet.Password(12))
                    .RuleFor(a => a.Deleted, f => f.Random.Bool())
                    .RuleFor(a => a.PasswordHash, f => f.Random.AlphaNumeric(32))
                    .RuleFor(a => a.PasswordSalt, f => f.Random.AlphaNumeric(16))
                    .RuleFor(a => a.AccountId, 123456)
                    .RuleFor(a => a.InsertDate, f => f.Date.Between(startDate, endDate));
        return accessFaker;
    }

    public static Faker<AccessWeb> GetAccessWebFaker()
    {
        var accesswebFaker = new Faker<AccessWeb>()
             .RuleFor(a => a.AccessID, f => f.Random.Long(100000,10000000))
             .RuleFor(a => a.AccessName, f => f.Name.FullName())
             .RuleFor(a => a.WebBackground, f => f.Random.String())
             .RuleFor(a => a.SalesPassword, f => f.Random.Bool())
             .RuleFor(a => a.ResetToken, f => f.Random.AlphaNumeric(32));
        return accesswebFaker;
    }
    
}
