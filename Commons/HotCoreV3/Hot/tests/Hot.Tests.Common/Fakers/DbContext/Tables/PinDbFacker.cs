using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class PinDbFacker
{
    public static IDbContext RandomPinStockList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Pins.Stock().Returns(GetPinStockList(count));
        service.dbContext.Pins.StockAsync().Returns(GetPinStockList(count));
        return service.dbContext;
    }
    public static IDbContext RandomPromoStockList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Pins.PromoStock().Returns(GetPinStockList(count));
        service.dbContext.Pins.PromoStockAsync().Returns(GetPinStockList(count));
        return service.dbContext;
    }
    public static IDbContext RandomPromoRechargesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Pins.PromoRecharge(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<decimal>(), 1, Arg.Any<string>()).Returns(GetPinList(count));
        service.dbContext.Pins.PromoRechargeAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<decimal>(), 1, Arg.Any<string>()).Returns(GetPinList(count));
        return service.dbContext;
    }
    public static IDbContext PromoHasPurchased(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.Pins.PromoHasPurchased(Arg.Any<long>()).Returns(expectedResult);
        service.dbContext.Pins.PromoHasPurchasedAsync(Arg.Any<long>()).Returns(expectedResult);
        return service.dbContext;
    }

    public static IDbContext RandomStockLoadedInBatch(this DbFakerService service, int count = 5)
    {
        service.dbContext.Pins.StockLoadedInBatchAsync(Arg.Any<int>()).Returns(GetPinStockList(count));
        service.dbContext.Pins.StockLoadedInBatch(Arg.Any<int>()).Returns(GetPinStockList(count));
        return service.dbContext;
    }

    public static IDbContext SavePin(this DbFakerService service, int expectedResult)
    {
        service.dbContext.Pins.AddAsync(Arg.Any<Pin>()).Returns(expectedResult);
        return service.dbContext;
    }



    public static Pin GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Pin> GetPinList(int count)
    {
        return GetFaker().Generate(count);
    }
    public static List<PinStockModel> GetPinStockList(int count)
    {
        return GetPinStockFaker().Generate(count);
    }
    public static PinStockModel GetPinStockSingle()
    {
        return GetPinStockFaker().Generate();
    }

    public static PinFileDataModel GetPinFileData()
    {
        return GetPinModelFaker().Generate();
    }


    private static Faker<Pin> GetFaker()
    {
        var pinFaker = new Faker<Pin>()
            .RuleFor(a => a.PinId, f => f.Random.Long())
            .RuleFor(a => a.PinBatchId, f => f.Random.Long())
            .RuleFor(a => a.PinStateId, f => f.Random.Byte())
            .RuleFor(a => a.BrandId, f => f.Random.Byte())
            .RuleFor(a => a.PinRef, f => f.Random.String())
            .RuleFor(a => a.PinValue, f => f.Random.Decimal())
            .RuleFor(a => a.PinExpiry, DateTime.Now);
        return pinFaker;
    }
    public static Faker<PinStockModel> GetPinStockFaker()
    {
        var pinsFaker = new Faker<PinStockModel>()
              .RuleFor(a => a.Brandid, f => f.Random.Int())
              .RuleFor(a => a.BrandName, f => f.Random.String())
              .RuleFor(a => a.PinValue, f => f.Random.Decimal())
              .RuleFor(a => a.Stock, f => f.Random.Decimal());
        return pinsFaker;
    }
    public static Faker<PinFileDataModel> GetPinModelFaker()
    {
        var pinFaker = new Faker<PinFileDataModel>()
            .RuleFor(a => a.BatchNumber, f => f.Random.String())
            .RuleFor(a => a.SourceReference, f => f.Random.String())
            .RuleFor(a => a.PinBatchType, f => f.Random.Int())
             .RuleFor(a => a.Quantity, f => f.Random.Int());
        return pinFaker;
    }
}


