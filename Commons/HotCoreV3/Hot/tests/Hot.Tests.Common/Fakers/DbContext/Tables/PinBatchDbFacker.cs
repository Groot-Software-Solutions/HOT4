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

public static class PinBatchDbFacker
{
    public static IDbContext RandomPinBatchesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.PinBatches.List(Arg.Any<int>()).Returns(GetList(count));
        service.dbContext.PinBatches.ListAsync(Arg.Any<int>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext RandomPinBatcheTypesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.PinBatchTypes.List().Returns(GetPinBatchTypeList(count));
        service.dbContext.PinBatchTypes.ListAsync().Returns(GetPinBatchTypeList(count));
        return service.dbContext;
    }
    public static PinBatch GetSingle()
    {
        return GetFaker().Generate();
    }
    public static List<PinBatch> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    public static PinBatchType GetPinBatchTypeSingle()
    {
        return GetPinBatchFaker().Generate();
    }
    public static List<PinBatchType> GetPinBatchTypeList(int count)
    {
        return GetPinBatchFaker().Generate(count);
    }
    private static Faker<PinBatch> GetFaker()
    {
        var pinBatchFaker = new Faker<PinBatch>()
            .RuleFor(a => a.PinBatchId, f => f.Random.Int())
            .RuleFor(a => a.BatchDate, DateTime.Now)
            .RuleFor(a => a.PinBatchTypeId, f => f.Random.Byte());
        return pinBatchFaker;
    }
    private static Faker<PinBatchType> GetPinBatchFaker()
    {
        var pinbatchTypeFaker = new Faker<PinBatchType>()
         .RuleFor(a => a.PinBatchTypeId, f => f.Random.Byte())
         .RuleFor(a => a.PinBatchTypeText, f => f.Random.String());
        return pinbatchTypeFaker;
    }
}

