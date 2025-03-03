using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class ProfileDbFacker

{
    public static Profile GetSingle()
    {
        return GetFaker().Generate();
    }
    public static List<Profile> GetProfilesList(int count)
    {
        return GetFaker().Generate(count);
    }
    public static IDbContext RandomProfilesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Profiles.List().Returns(GetProfilesList(count));
        service.dbContext.Profiles.ListAsync().Returns(GetProfilesList(count));
        return service.dbContext;
    }
    public static IDbContext RandomProfileDiscount(this DbFakerService service)
    {
  
        service.dbContext.ProfileDiscounts.DiscountAsync(Arg.Any<int>(), Arg.Any<int>()).Returns(GetProfileDiscountSingle());
        return service.dbContext;
    }
    public static ProfileDiscount GetProfileDiscountSingle()
    {
        return GetProfileDiscountFaker().Generate();
    }
    public static List<ProfileDiscount> GetProfileDiscountList(int count)
    {
        return GetProfileDiscountFaker().Generate(count);
    }

    private static Faker<Profile> GetFaker()
    {
        var profileFaker = new Faker<Profile>()
            .RuleFor(a => a.ProfileId, f => f.Random.Int())
            .RuleFor(a => a.ProfileName, f => f.Random.String());
        return profileFaker;
    }
    private static Faker<ProfileDiscount> GetProfileDiscountFaker()
    {
        var profileDiscountFaker = new Faker<ProfileDiscount>()
            .RuleFor(a => a.ProfileId, f => f.Random.Int())
            .RuleFor(a => a.BrandId, f => f.Random.Byte())
            .RuleFor(a => a.Discount, f => f.Random.Decimal())
            .RuleFor(a => a.ProfileDiscountId, f => f.Random.Int());

        return profileDiscountFaker;
    }
}

