using Bogus;
using FluentAssertions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class BrandDbFacker
{
    public static IDbContext RandomBrandsList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Brands.List().Returns(GetList(count));
        service.dbContext.Brands.ListAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext BrandIdentify(this DbFakerService service, int brandId)
    { 
     
        service.dbContext.Brands.IndentifyAsync(Arg.Any<int>(), Arg.Any<string>()).Returns(brandId);
        return service.dbContext;
    }
    public static IDbContext Brand(this DbFakerService service, Brand brand)
    {
        service.dbContext.Brands.Get(Arg.Any<int>()).Returns(brand);
        service.dbContext.Brands.GetAsync(Arg.Any<int>()).Returns(brand);

        return service.dbContext;
    }
    public static IDbContext RandomBrand(this DbFakerService service)
    {
        return service.Brand(GetSingle());
    }
    public static Brand GetSingle()
    {
        return GetFaker().Generate();
    }
    public static List<Brand> GetList(int count)
    {
        return GetFaker().Generate(count);
    }

    private static Faker<Brand> GetFaker()
    {
        var brandFaker = new Faker<Brand>()
            .RuleFor(a => a.BrandId, f => f.Random.Byte(1,100))
            .RuleFor(a => a.NetworkId, f => f.Random.Byte(1, 100))
            .RuleFor(a => a.BrandName, f => f.Company.CompanyName())
            .RuleFor(a => a.BrandSuffix, f => f.Random.AlphaNumeric(1))
            .RuleFor(a => a.WalletTypeId, f=>f.Random.Int(1,3));
        return brandFaker;
    }

}

