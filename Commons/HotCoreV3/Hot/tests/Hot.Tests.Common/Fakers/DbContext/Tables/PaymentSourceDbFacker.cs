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

    public static class PaymentSourceDbFacker
    {
    public static IDbContext RandomPaymentSourcesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.PaymentSources.List().Returns(GetPaymentSourcesList(count));
        service.dbContext.PaymentSources.ListAsync().Returns(GetPaymentSourcesList(count));
        return service.dbContext;
    }
    public static List<PaymentSource> GetPaymentSourcesList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<PaymentSource> GetFaker()
    {
        var paymentSourceFaker = new Faker<PaymentSource>()
         .RuleFor(a => a.PaymentSourceId, f => f.Random.Byte(1, 10))
         .RuleFor(a => a.PaymentSourceText, f => f.Random.String());
        return paymentSourceFaker;
    }
}

