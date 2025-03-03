using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Extensions;
using Infrastructure.Services.Mail;
using InfrastructureTests.Common;
using Microsoft.Extensions.Options;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace InfrastructureTests.Services
{
    public class MailServiceTests
    {
        readonly IMailService mailService;
       public MailServiceTests()
        {
            var pbx = new Mock<IOptions<PBXOptions>>();
            var mail = new Mock<IOptions<MailServiceOptions>>();
            mailService = new MailKitService(FakeObjects.GetDbContext().Object,mail.Object,pbx.Object);
        }

        [Fact]
        public void GetSMSTests ()
        {

            var testdata = new Message() {
                 Body= "test"
                 , From="support@hot.co.zw"
                 , Received = DateTime.Parse("09/01/2020 00:00:00")
                 , Subject= "SMS receive from +263719413755 at 2020-08-30 20:15:11"
                 , To = "support@hot.co.zw"
            };

            var result = mailService.GetSMS(testdata);
            result.Mobile.ShouldBe("0719413755");
            result.DirectionId.ShouldBe(Domain.Enums.SMSDirection.In);
            result.Text.ShouldBe("test"); 

        }
       
    }
}
