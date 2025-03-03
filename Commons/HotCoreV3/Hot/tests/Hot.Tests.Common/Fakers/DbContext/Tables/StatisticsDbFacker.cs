using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class StatisticsDbFacker
{
    public static IDbContext RandomRechargeTraffic(this DbFakerService service, int count = 5)
    {
        service.dbContext.Statistics.GetRechargeTrafficAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RandomRechargeTrafficLastDay(this DbFakerService service, int count = 5)
    {
        service.dbContext.Statistics.GetRechargeTrafficLastDayAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RandomRechargeLatestTraffic(this DbFakerService service, int count = 5)
    {
        service.dbContext.Statistics.GetRechargeTrafficAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RandomSMSTraffic(this DbFakerService service, int count = 5)
    {
        service.dbContext.Statistics.GetSmsTrafficAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static List<StatResult> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<StatResult> GetFaker()
    {
        var statResultFaker = new Faker<StatResult>()
            .RuleFor(a => a.Name, f => f.Name.ToString())
            .RuleFor(a => a.X, f => f.Random.Decimal())
            .RuleFor(a => a.Y, f => f.Random.Decimal());
        return statResultFaker;
    }
}

