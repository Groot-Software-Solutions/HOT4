using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
//using Hot.Ecocash.Application.Common.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hot.Tests.Common.Fakers.DbContext.Tables
{
    public static class SelfTopUpDbFacker
    {
        public static IDbContext SelfTopupAdd(this DbFakerService service)
        {
            service.dbContext.SelfTopUps.Add(Arg.Any<SelfTopUp>()).Returns(123);
            service.dbContext.SelfTopUps.AddAsync(Arg.Any<SelfTopUp>()).Returns(123);
            return service.dbContext;
        }
        public static IDbContext SelftopupUpdate(this DbFakerService service, bool expectedResult)
        {
            service.dbContext.SelfTopUps.Update(Arg.Any<SelfTopUp>()).Returns(expectedResult);
            service.dbContext.SelfTopUps.UpdateAsync(Arg.Any<SelfTopUp>()).Returns(expectedResult);
            return service.dbContext;
        }
        //public static SelfTopUpRequest GetSingle()
        //{
        //    return GetFaker().Generate();
        //}

        //public static List<SelfTopUpRequest> GetList(int count)
        //{
        //    return GetFaker().Generate(count);
        //}
        //private static Faker<SelfTopUpRequest> GetFaker()
        //{
        //    var promotionRechargePinSelfTopUpRequestFaker = new Faker<SelfTopUpRequest>()
        //            .RuleFor(a => a.TargetMobile, "0773404368")
        //            .RuleFor(a => a.AccessCode, "0775085496")
        //            .RuleFor(a => a.Amount, 500);
        //    return promotionRechargePinSelfTopUpRequestFaker;
        //}

    }
}

