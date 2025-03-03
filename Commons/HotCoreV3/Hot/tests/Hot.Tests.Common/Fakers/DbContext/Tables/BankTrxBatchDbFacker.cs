using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class BankTrxBatchDbFacker
{
    public static IDbContext RandomBankTrxBatchList(this DbFakerService service, int count = 5)
    {
        service.dbContext.BankTrxBatches.List(Arg.Any<byte>()).Returns(GetList(count));
        service.dbContext.BankTrxBatches.ListAsync(Arg.Any<byte>()).Returns(GetList(count));
        return service.dbContext;
    }
    public static IDbContext GetCurrentBatch(this DbFakerService service, BankTrxBatch bankTrxBatch)
    {
        service.dbContext.BankTrxBatches.GetCurrentBatch(Arg.Any<BankTrxBatch>()).Returns(bankTrxBatch);
        service.dbContext.BankTrxBatches.GetCurrentBatchAsync(Arg.Any<BankTrxBatch>()).Returns(bankTrxBatch);
        return service.dbContext;
    }
    public static IDbContext RandomBankTrxBatch(this DbFakerService service)
    {
        return BankTrxBatch(service, GetSingle());
    }

    public static IDbContext BankTrxBatch(this DbFakerService service, BankTrxBatch bankTrxBatch)
    {
        service.dbContext.BankTrxBatches.Get(Arg.Any<int>()).Returns(bankTrxBatch);
        service.dbContext.BankTrxBatches.GetAsync(Arg.Any<int>()).Returns(bankTrxBatch);
        return service.dbContext;
    }

    public static BankTrxBatch GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<BankTrxBatch> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<BankTrxBatch> GetFaker()
    {
        DateTime startDate = Convert.ToDateTime("01-01-2008");
        DateTime endDate = DateTime.Now;
        var bankTrxBatchFaker = new Faker<BankTrxBatch>()
           .RuleFor(a => a.BankID, f => f.Random.Int(1,100))
           .RuleFor(a => a.BankTrxBatchID, f => f.Random.Long(100000,10000000))
           .RuleFor(a => a.BatchDate, f => f.Date.Between(startDate, endDate))
           .RuleFor(a => a.BatchReference, f => f.Random.AlphaNumeric(20))
           .RuleFor(a => a.LastUser, f => f.Internet.UserName());
        return bankTrxBatchFaker;
    }
}

