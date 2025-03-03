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

public static class BankTrxDbFacker
{
    public static IDbContext RandomBankTrxList(this DbFakerService service, int count = 5)
    {
        service.dbContext.BankTrxs.List(Arg.Any<long>()).Returns(GetList(count));
        service.dbContext.BankTrxs.ListAsync(Arg.Any<long>()).Returns(GetList(count));
        return service.dbContext;
    }

    public static IDbContext RandomBankTrx(this DbFakerService service)
    {
        return BankTrx(service, GetSingle());
    }

    public static IDbContext BankTrx(this DbFakerService service, BankTrx bankTrx)
    {
        service.dbContext.BankTrxs.Get(Arg.Any<int>()).Returns(bankTrx);
        service.dbContext.BankTrxs.GetAsync(Arg.Any<int>()).Returns(bankTrx);
        return service.dbContext;
    }
    public static IDbContext BankTrxUpdate(this DbFakerService service, bool expectedResult)
    {
        service.dbContext.BankTrxs.Update(Arg.Any<BankTrx>()).Returns(expectedResult);
        service.dbContext.BankTrxs.UpdateAsync(Arg.Any<BankTrx>()).Returns(expectedResult);
        return service.dbContext;
    }
    public static IDbContext BankTrxAdd(this DbFakerService service)
    {
        service.dbContext.BankTrxs.Add(Arg.Any<BankTrx>()).Returns(123);
        service.dbContext.BankTrxs.AddAsync(Arg.Any<BankTrx>()).Returns(123);
        return service.dbContext;
    }
    public static BankTrx GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<BankTrx> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<BankTrx> GetFaker()
    {
        DateTime startDate = Convert.ToDateTime("01-01-2008");
        DateTime endDate = DateTime.Now;
        var bankTrxfaker = new Faker<BankTrx>()
            .RuleFor(a => a.BankTrxID, f => f.Random.Long(1000000,10000000))
            .RuleFor(a => a.BankTrxBatchID, f => f.Random.Long(100000,10000000))
            .RuleFor(a => a.BankTrxTypeID, f => f.Random.Byte(1,100))
            .RuleFor(a => a.BankTrxStateID, f => f.Random.Byte(1,20))
            .RuleFor(a => a.Amount, f => f.Random.Decimal())
            .RuleFor(a => a.TrxDate, f => f.Date.Between(startDate, endDate))
            .RuleFor(a => a.Identifier, f => f.Phone.PhoneNumber())
            .RuleFor(a => a.RefName, f => f.Random.AlphaNumeric(12))
            .RuleFor(a => a.Branch, f => f.Company.CompanyName())
            .RuleFor(a => a.BankRef, f => f.Random.AlphaNumeric(20))
            .RuleFor(a => a.Balance, f => f.Random.Decimal())
            .RuleFor(a => a.PaymentID, f => f.Random.Long(1000,10000000));
        return bankTrxfaker;
    }
}
