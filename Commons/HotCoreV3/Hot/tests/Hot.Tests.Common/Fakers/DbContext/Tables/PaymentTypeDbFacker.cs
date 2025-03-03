using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;


namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class PaymentTypeDbFacker
{
    public static IDbContext RandomPaymentTypesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.PaymentTypes.List().Returns(GetList(count));
        service.dbContext.PaymentTypes.ListAsync().Returns(GetList(count));
        return service.dbContext;
    }


    public static List<PaymentType> GetList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<PaymentType> GetFaker()
    {
        var paymentTypeFaker = new Faker<PaymentType>()
           .RuleFor(a => a.PaymentTypeId, f => f.Random.Byte(1, 10))
           .RuleFor(a => a.PaymentTypeText, f => f.Random.String());
        return paymentTypeFaker;
    }
}

