using Bogus;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class LimitDbFacker

{
    public static IDbContext AccountLimit(this DbFakerService service, LimitModel limitModel)
    {
  
        service.dbContext.Limits.GetCurrentLimitsAsync(Arg.Any<int>() , Arg.Any<long>()).Returns(limitModel);
        return service.dbContext;
    }

    public static IDbContext LimitAddSuccessful(this DbFakerService service)
    {
        service.dbContext.Limits.Add(Arg.Any<Limit>()).Returns(1);
        service.dbContext.Limits.AddAsync(Arg.Any<Limit>()).Returns(1);
        return service.dbContext;
    }
    public static IDbContext LimitAddFailed(this DbFakerService service)
    {
        service.dbContext.Limits.Add(Arg.Any<Limit>()).Returns(new HotDbException("Test Failed","Error Occured"));
        service.dbContext.Limits.AddAsync(Arg.Any<Limit>()).Returns(new HotDbException("Test Failed","Error Occured"));
        return service.dbContext;
    }
    public static Limit GetSingle()
    {
        return GetFaker().Generate();
    }

    private static Faker<Limit> GetFaker()
    {
        var limitFaker = new Faker<Limit>()
          .RuleFor(a => a.LimitId, f => f.Random.Long())
          .RuleFor(a => a.NetworkId, f => f.Random.Byte())
          .RuleFor(a => a.AccountId, f => f.Random.Long())
          .RuleFor(a => a.LimitTypeId, f => f.Random.Int())
          .RuleFor(a => a.DailyLimit, f => f.Random.Decimal())
          .RuleFor(a => a.Monthly, f => f.Random.Decimal());

        return limitFaker; 
    }

    public static Faker<LimitModel> GetLimit()
    {

        var limitFaker = new Faker<LimitModel>()
          .RuleFor(a => a.RemainingLimit, f => f.Random.Decimal())
          .RuleFor(a => a.RemainingDailyLimit, f => f.Random.Decimal())
          .RuleFor(a => a.RemainingMonthlyLimit, f => f.Random.Decimal())
          .RuleFor(a => a.DailyLimit, 12)
          .RuleFor(a => a.MonthlyLimit, f => f.Random.Decimal())
          .RuleFor(a => a.SalesToday, f => f.Random.Decimal())
          .RuleFor(a => a.SalesMonthly, f => f.Random.Decimal())
          .RuleFor(a => a.LimitTypeId, f => f.Random.Byte());


        return limitFaker;
    }

}

