using Bogus;
//using Hot.API.Service.Models.Accounts;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace Hot.Tests.Common.Fakers.DbContext.Tables;

public static class PaymentDbFacker
{
    public static IDbContext RandomPaymentsList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Payments.ListAsync().Returns(GetPaymentsList(count));
        return service.dbContext;
    }
    public static IDbContext PaymentsList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Payments.SearchAsync(Arg.Any<String>()).Returns(GetPaymentsList(count));
        return service.dbContext;
    }
    public static IDbContext RandomPaymentsByAccountIdList(this DbFakerService service, int count = 5)
    {
        service.dbContext.Payments.ListAsync(Arg.Any<int>()).Returns(GetPaymentsList(count));
        return service.dbContext;
    }
    public static IDbContext RandomPaymentsRecentList(this DbFakerService service, int count = 5)
    {
    
        service.dbContext.Payments.ListRecentAsync(Arg.Any<int>()).Returns(GetPaymentsList(count));
        return service.dbContext;
    }
    public static IDbContext RandomPayment(this DbFakerService service)
    {
        return Payment(service, GetSingle());
    }

    public static IDbContext Payment(this DbFakerService service, Payment payment)
    {
        service.dbContext.Payments.GetAsync(Arg.Any<int>()).Returns(payment);
        return service.dbContext;
    }
    public static IDbContext PaymentAdd(this DbFakerService service,int paymentId)
    {
        service.dbContext.Payments.AddAsync(Arg.Any<Payment>()).Returns(paymentId);
        return service.dbContext;
    }
    public static Payment GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<Payment> GetPaymentsList(int count)
    {
        return GetFaker().Generate(count);
    }
    //private static Faker<PaymentModel> GetModelsFaker()
    //{
    //    var paymentModelsFaker = new Faker<PaymentModel>().RuleFor(a => a.PaymentId, 12)
    //    //.RuleFor(a => a.PaymentSourceId, f => f.Random.Byte())
    //    .RuleFor(a => a.PaymentDate, DateTime.Now)
    //    .RuleFor(a => a.Reference, f => f.Random.String());
    //    //.RuleFor(a => a.LastUser, f => f.Random.String());
    //    return paymentModelsFaker;
    //}

    private static Faker<Payment> GetFaker()
    {
        var paymentFaker = new Faker<Payment>()
        .RuleFor(a => a.PaymentId, 12)
        .RuleFor(a => a.AccountId, f => f.Random.Long())
        .RuleFor(a => a.PaymentTypeId, f => f.Random.Byte(1,10))
        .RuleFor(a => a.Amount, f => f.Random.Decimal())
        .RuleFor(a => a.PaymentSourceId, f => f.Random.Byte(1,10))
        .RuleFor(a => a.PaymentDate, DateTime.Now)
        .RuleFor(a => a.Reference, f => f.Random.String())
        .RuleFor(a => a.LastUser, f => f.Random.String());
        return paymentFaker;
    }
}
