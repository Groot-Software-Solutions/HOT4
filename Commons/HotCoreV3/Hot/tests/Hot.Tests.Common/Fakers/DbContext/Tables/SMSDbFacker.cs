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

public static class SMSDbFacker
{   
    public static IDbContext RandomList(this DbFakerService service, int count = 5)
    {
        service.dbContext.SMSs.Search(Arg.Any<string>()).Returns(GetSMSList(count));
        service.dbContext.SMSs.SearchAsync(Arg.Any<string>()).Returns(GetSMSList(count));
        return service.dbContext;
    }
    public static IDbContext RandomSMSsSearchByDatesList(this DbFakerService service, int count = 5)
    {
        service.dbContext.SMSs.SearchByDates(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetSMSList(count));
        service.dbContext.SMSs.SearchByDatesAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>()).Returns(GetSMSList(count));
        return service.dbContext;
    }
    public static IDbContext RandomSMSSearchByFilterList(this DbFakerService service, int count = 5)
    {
        service.dbContext.SMSs.SearchByFilter(Arg.Any<string>()).Returns(GetSMSList(count));
        service.dbContext.SMSs.SearchByFilterAsync(Arg.Any<string>()).Returns(GetSMSList(count));
        return service.dbContext;
    }
    public static IDbContext RandomSMSSearchByMobileList(this DbFakerService service, int count = 5)
    {
        service.dbContext.SMSs.SearchByMobile(Arg.Any<string>()).Returns(GetSMSList(count));
        service.dbContext.SMSs.SearchByMobileAsync(Arg.Any<string>()).Returns(GetSMSList(count));
        return service.dbContext;
    }
    public static IDbContext RandomSMS(this DbFakerService service)
    {
        return SMS(service, GetSingle());
    }

    public static IDbContext SMS(this DbFakerService service, SMS sms)
    {
        service.dbContext.SMSs.GetAsync(Arg.Any<int>()).Returns(sms);
        return service.dbContext;
    }
    public static IDbContext SMSAdd(this DbFakerService service, int smsId)
    {
        service.dbContext.SMSs.AddAsync(Arg.Any<SMS>()).Returns(smsId);
        return service.dbContext;
    }
    public static IDbContext SMSInbox(this DbFakerService service, int count)
    {
        service.dbContext.SMSs.InboxAsync().Returns(GetSMSList(count));
        return service.dbContext;
    }
    public static SMS GetSingle()
    {
        return GetFaker().Generate();
    }

    public static List<SMS> GetSMSList(int count)
    {
        return GetFaker().Generate(count);
    }
    private static Faker<SMS> GetFaker()
    {
        var smsFaker = new Faker<SMS>()
             .RuleFor(a => a.SMSID, f => f.Random.Long())
             .RuleFor(a => a.SmppID, f => f.Random.Int())
             .RuleFor(a => a.Direction, f => f.Random.Bool())
             .RuleFor(a => a.Mobile, f => f.Random.String())
             .RuleFor(a => a.SMSText, f => f.Random.String())
             .RuleFor(a => a.SMSDate, DateTime.Now)
             .RuleFor(a => a.SMSID_In, f => f.Random.Long())
             .RuleFor(a => a.InsertDate, DateTime.Now);
        return smsFaker;
    }
}

