using Domain.Enum;
using Domain.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using System.Text.RegularExpressions;
using Twilio.AspNet.Common;
using Xunit;

namespace WhatsAppTests.Services
{
    public class TwilioServiceTests
    {
        readonly IConfigHelper configHelper;
        readonly TwilioService service;
        public TwilioServiceTests()
        {
            configHelper = (new Mock<IConfigHelper>()).Object;
            service = new TwilioService(configHelper);
        }

        [Fact]
        public void NumberFomatTests()
        { 
            service.NumberFormat("0772397464").ShouldBe("+263772397464");
            service.NumberFormat("772397464").ShouldBe("+263772397464"); 
            //service.NumberFormat("263772397464").ShouldBe("+263772397464");
             
        }

        [Fact]
        public void GetMessageTest()
        {
            var testdata = new SmsRequest()
            {
                SmsSid = "TestID"
                 , Body = "Test Message"
                 , From = "whatsapp:+263772397464"
                 , To = "whatsapp:+14155238886"
                 , MessageStatus = "Pending"
            };

            var result = service.GetMessage(testdata);

            result.PhoneNumber.ShouldBe("0772397464");
            result.Text.ShouldBe("Test Message");
            result.Type.ShouldBe(MessageType.Text);
            result.ConversationId.ShouldBe("TestID");
             


        }



    }
}
