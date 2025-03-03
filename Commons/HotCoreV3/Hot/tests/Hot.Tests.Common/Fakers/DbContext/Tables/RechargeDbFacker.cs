using Bogus;
using Hot.Application.Common.Exceptions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using NSubstitute;
using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;
public static class RechargeDbFacker
{
    public static IDbContext RandomRechargeList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Recharges.Search(Arg.Any<string>()).Returns(GetList(count));
        service.dbContext.Recharges.SearchAsync(Arg.Any<string>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RechargeAdd(this DbFakerService service, int rechargeId = 123)
    {
        service.dbContext.Recharges.Add(Arg.Any<Recharge>()).Returns(rechargeId);
        service.dbContext.Recharges.AddAsync(Arg.Any<Recharge>()).Returns(rechargeId);
        return service.dbContext;
    }
    public static IDbContext FindByMobile(this DbFakerService service, int count)
    {
        service.dbContext.Recharges.FindByMobileAsync(Arg.Any<string>()).Returns(GetRechargeModelList(count));
        return service.dbContext;
    }

    public static IDbContext RandomRechargeSelectAgrregatorList(this DbFakerService service, int count = 5)
    {

        service.dbContext.Recharges.SelectAggregatorAsync(Arg.Any<long>(), Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RandomRechargesFindByAccount(this DbFakerService service, int count = 5)
    {
        service.dbContext.Recharges.FindByAccountAsync(Arg.Any<long>(), Arg.Any<string>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RandomRecharge(this DbFakerService service)
    {
        return Recharge(service, GetSingle());
    }

    public static IDbContext Recharge(this DbFakerService service, Recharge recharge)
    {
        service.dbContext.Recharges.GetAsync(Arg.Any<int>()).Returns(recharge);
        return service.dbContext;
    }

    public static Recharge GetSingle()
    {
        return GetFaker().Generate();
    }
    public static RechargeResult GetRechargeResult()
    {
        return GetRechargeResultFaker().Generate();
    }
    public static List<Recharge> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    public static List<RechargeResultModel> GetRechargeModelList(int count)
    {
        return GetRechargeModelFaker().Generate(count);
    }
    private static Faker<Recharge> GetFaker()
    {
        DateTime startDate = Convert.ToDateTime("01-01-2008");
        DateTime endDate = DateTime.Now;
        var rechargeFaker = new Faker<Recharge>()
            .RuleFor(a => a.RechargeId, f => f.Random.Long())
            .RuleFor(a => a.StateId, f => f.Random.Byte())
            .RuleFor(a => a.AccessId, f => f.Random.Long())
            .RuleFor(a => a.Amount, f => f.Random.Decimal())
            .RuleFor(a => a.Discount, f => f.Random.Decimal())
            .RuleFor(a => a.Mobile, f => f.Random.String())
            .RuleFor(a => a.BrandId, f => f.Random.Byte())
            .RuleFor(a => a.RechargeDate, f => f.Date.Between(startDate, endDate))
            .RuleFor(a => a.InsertDate, f => f.Date.Between(startDate, endDate));
        return rechargeFaker;
    }
    private static Faker<RechargeResult> GetRechargeResultFaker()
    {
        var rechargeResultFaker = new Faker<RechargeResult>()
            .RuleFor(a => a.Successful, true)
            .RuleFor(a => a.Message, f => f.Random.String())
            .RuleFor(a => a.ErrorData, f => f.Random.String())
            .RuleFor(a => a.Data, "")
            .RuleFor(a => a.Recharge, f => RechargeDbFacker.GetSingle())
            .RuleFor(a => a.RechargePrepaid, RechargePrepaidDbFacker.GetSingle())
            .RuleFor(a => a.WalletBalance, f => f.Random.Decimal());
        return rechargeResultFaker;
    }
    private static Faker<RechargeResultModel> GetRechargeModelFaker()
    {
        DateTime startDate = Convert.ToDateTime("01-01-2008");
        DateTime endDate = DateTime.Now;
        var rechargeModelFaker = new Faker<RechargeResultModel>()
            .RuleFor(a => a.RechargeId, f => f.Random.Long())
            .RuleFor(a => a.StateId, f => f.Random.Byte())
            .RuleFor(a => a.AccessId, f => f.Random.Long())
            .RuleFor(a => a.Amount, f => f.Random.Decimal())
            .RuleFor(a => a.Discount, f => f.Random.Decimal())
            .RuleFor(a => a.Mobile, f => f.Random.String())
            .RuleFor(a => a.BrandId, f => f.Random.Byte())
            .RuleFor(a => a.RechargeDate, f => f.Date.Between(startDate, endDate))
            .RuleFor(a => a.InsertDate, f => f.Date.Between(startDate, endDate));
        return rechargeModelFaker;
    }



}
