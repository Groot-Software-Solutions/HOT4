using Domain.DataModels;
using Domain.Interfaces;
using Infrastructure.Handlers;
using Moq;
using Domain.Enum;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Infrastructure.Extensions;
using Xunit;
using Shouldly;
using Infrastructure.Services;
using WhatsAppTests.Common;
using Domain.Entities;

namespace WhatsAppTests.HandlerTests
{
   public class MessageProcessorTests
    {
        readonly IDbContext dbcontext;
        readonly IDealerService dealerService;
        readonly IMessageProccessor messageProccessor;
        readonly IRechargeHelper rechargeService;
        readonly ITemplateHelper templateHelper;
        readonly ILogger logger;
        readonly IConfigHelper config;

        public MessageProcessorTests()
        {
            config = FakeObjects.GetConfig().Object;
            dbcontext = FakeObjects.GetDBContext().Object;
            dealerService = new Mock<IDealerService>().Object;
            rechargeService = new Mock<IRechargeHelper>().Object;
            templateHelper = new TemplateHelper(dbcontext);
            logger = new Mock<ILogger>().Object;
            messageProccessor = new MessageProccessor(rechargeService,dealerService,templateHelper,dbcontext,logger,config);
        }



        [Fact]
        public void GetSelfTopUpTest()
        {

            var testdata = "airtime#100*0772397464 0772812744";

            var result = messageProccessor.GetSelfTopUp(testdata);
            result.BillerMobile.ShouldBe("0772812744");
            result.TargetMobile.ShouldBe("0772397464");
            result.Amount.ShouldBe(100);

        }
        [Fact]
        public void GetSelfTopUpHotTest()
        {

            var testdata = "hot#100*0772397464 0772812744";

            var result = messageProccessor.GetSelfTopUp(testdata);
            result.BillerMobile.ShouldBe("0772812744");
            result.TargetMobile.ShouldBe("0772397464");
            result.Amount.ShouldBe(100);


        }
        [Fact]
        public void GetSelfTopUpWithoutAirtimeTest()
        {
            var testdata = "100*0772397464 0772812744";

            var result = messageProccessor.GetSelfTopUp(testdata);
            result.BillerMobile.ShouldBe("0772812744");
            result.TargetMobile.ShouldBe("0772397464");
            result.Amount.ShouldBe(100);

        }
        [Fact] 
        public void GetResetData()
        {
            //Standard format Long ID
            var testdata = "RESET SIBONENI CHITIMBE 66-2000452H66 0772621425";
            var testsender = "0772397464";
            var result = messageProccessor.GetPinReset(testdata,testsender);
            result.Sender.ShouldBe(testsender);
            result.TargetMobile.ShouldBe("0772621425");
            result.Names.ShouldBe("SIBONENI CHITIMBE"); 
            result.IDNumber.ShouldBe("66-2000452H66");

            // Self Reset Short IDNo Number 
            testdata = "reset bryan zulu 25-076288J25";
            testsender = "263772397464";
            result = messageProccessor.GetPinReset(testdata, testsender);
            result.Sender.ShouldBe("0772397464");
            result.TargetMobile.ShouldBe("0772397464");
            result.Names.ShouldBe("bryan zulu");
            result.IDNumber.ShouldBe("25-076288J25");
            // Middlename New lines
            testdata = @"reset bryan livingstone zulu 
25-076288J25";
            testsender = "263772397464";
            result = messageProccessor.GetPinReset(testdata, testsender);
            result.Sender.ShouldBe("0772397464");
            result.TargetMobile.ShouldBe("0772397464");
            result.Names.ShouldBe("bryan zulu livingstone");
            result.IDNumber.ShouldBe("25-076288J25");
            // Field Labels & reset pin
            testdata = "reset pin Name:bryan surname:zulu IdNumber:25-076288J25 mobile number: 0772397464";
            testsender = "263772397464";
            result = messageProccessor.GetPinReset(testdata, testsender);
            result.Sender.ShouldBe("0772397464");
            result.TargetMobile.ShouldBe("0772397464");
            result.Names.ShouldBe("bryan zulu");
            result.IDNumber.ShouldBe("25-076288J25");
        }
    }
}
