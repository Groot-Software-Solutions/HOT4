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

public static class BankDbFacker
{
    public static IDbContext RandomBanksList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Banks.List().Returns(GetList(count));
        service.dbContext.Banks.ListAsync().Returns(GetList(count));
        return service.dbContext;
    }
    public static Bank GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Bank> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<Bank> GetFaker()
    {
        var banksfaker = new Faker<Bank>()
             .RuleFor(a => a.BankID, f => f.Random.Byte(1,100))
             .RuleFor(a => a.Name, f => f.Company.CompanyName())
             .RuleFor(a => a.SageBankID, f => f.Random.Int(10000,11000));

        return banksfaker;
    }

}
