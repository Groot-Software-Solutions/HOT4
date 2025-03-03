using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class RechargePrepaidDbFacker
{  
    public static IDbContext RandomRechargePrepaid(this DbFakerService service)
    {
        return RechargePrepaid(service, GetSingle());
    }

    public static IDbContext RechargePrepaidAdd(this DbFakerService service, int rechargeId =  123)
    {
        service.dbContext.RechargePrepaids.Add(Arg.Any<RechargePrepaid>()).Returns(rechargeId);
        service.dbContext.RechargePrepaids.AddAsync(Arg.Any<RechargePrepaid>()).Returns(rechargeId);
        return service.dbContext;
    }
    public static IDbContext RechargePrepaid(this DbFakerService service, RechargePrepaid RechargePrepaid)
    {
        service.dbContext.RechargePrepaids.Get(Arg.Any<int>()).Returns(RechargePrepaid);
        service.dbContext.RechargePrepaids.GetAsync(Arg.Any<int>()).Returns(RechargePrepaid);
        return service.dbContext;
    }

    public static RechargePrepaid GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<RechargePrepaid> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<RechargePrepaid> GetFaker()
    {
        var startDate = Convert.ToDateTime("01-01-2008");
        var endDate = DateTime.Now;
        var rechargePrepaidResultFaker = new Faker<RechargePrepaid>()
           .RuleFor(a => a.RechargeId, f => f.Random.Long())
           .RuleFor(a => a.DebitCredit, f => f.Random.Bool())
           .RuleFor(a => a.ReturnCode, f => f.Random.ToString())
           .RuleFor(a => a.Narrative, f => f.Random.ToString())
           .RuleFor(a => a.InitialBalance, f => f.Random.Decimal())
           .RuleFor(a => a.FinalBalance, f => f.Random.Decimal())
           .RuleFor(a => a.Reference, f => f.Random.ToString())
           .RuleFor(a => a.InitialWallet, f => f.Random.Decimal())
           .RuleFor(a => a.FinalWallet, f => f.Random.Decimal())
           .RuleFor(a => a.Window, f => f.Date.Between(startDate, endDate))
           .RuleFor(a => a.DebitCredit, f => f.Random.Bool())
           .RuleFor(a => a.ReturnCode, f => f.Random.ToString());
        return rechargePrepaidResultFaker;
    }

}
