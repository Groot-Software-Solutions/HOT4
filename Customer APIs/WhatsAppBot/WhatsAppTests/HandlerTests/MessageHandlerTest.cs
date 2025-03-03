using Domain.Enum;
using Domain.Interfaces;
using Infrastructure.Handlers;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Shouldly;
using System.Configuration;
using System.Text.RegularExpressions;
using WhatsAppTests.Common;
using Xunit;

namespace WhatsAppTests.HandlerTests
{
    public class MessageHandlerTest
    {
        readonly IDbContext dbcontext;
        readonly IConfigHelper helper;
        readonly IMessageHandler _messageHandler;
        readonly ILogger logger;

        public MessageHandlerTest()
        {

            helper = FakeObjects.GetConfig().Object; 
            dbcontext = new Mock<IDbContext>().Object; 

            _messageHandler = new MessageHandler(dbcontext, helper,logger);
        }

        #region "   Airtime Identification Tests  "
        [Fact]
        public void ShouldIdentifyAirtime()
        { 
            var testdata = "airtime#10#0772397464#0772397464";
            var expected = RequestType.Airtime;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }
         
        [Fact]
        public void ShouldIdentifyAirtime_TopUp()
        {
            var testdata = "topup 10 0772397464 0772397464";
            var expected = RequestType.Airtime;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }

        [Fact]
        public void ShouldIdentifyAirtimeWithoutAirtime()
        {
            var testdata = "10#0772397464#0772397464";
            var expected = RequestType.Airtime;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }

        #endregion

        #region "   Pin Reset Identification Tests   "
        [Fact]
        public void ShouldIdentifyPinReset()
        {
           
            var testdata = "Pin Bryan Zulu 12-076228J25";
            var expected = RequestType.PinReset;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }

        [Fact]
        public void ShouldIdentifyPinResetByReset()
        {
           
            var testdata = "reset Bryan Zulu 12-076228J25";
            var expected = RequestType.PinReset;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }

        #endregion

        #region "   Help Identification Tests   "
        
        [Fact]
        public void ShouldIdentifyHelp()
        {

            var testdata = "help with stuff";
            var expected = RequestType.Help;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }
        [Fact]
        public void ShouldIdentifyHelpBalance()
        {
           
            var testdata = "and how d l request my balance hot recharge account";
            var expected = RequestType.HelpBalance;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }
        [Fact]
        public void ShouldIdentifyHelpBank()
        {
           
            var testdata = "Gud morning may I have hot recharge banking details want to deposit my float";
            var expected = RequestType.HelpBank;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }
        [Fact]
        public void ShouldIdentifyHelpEcocash()
        {
           
            var testdata = "how do i put ecocash in my hot";
            var expected = RequestType.HelpEcocash;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }
        [Fact]
        public void ShouldIdentifyHelpRegister()
        {
           
            var testdata = "How to get registered for hot recharge services?";
            var expected = RequestType.HelpRegistration;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }






        #endregion

        #region "   Greetings Identifications Tests   "

        [Fact]
        public void ShouldIdentifyGreeting()
        {

            var testdata = "hello";
            var expected = RequestType.Greeting;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }
        [Fact]
        public void ShouldIdentifySignOff()
        {

            var testdata = "thank you";
            var expected = RequestType.SigningOff;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }


        #endregion

        [Fact]
        public void ShouldNotIdentifyAnything()
        {
           
            var testdata = "invalid data here now ";
            var expected = RequestType.Unknown;
            var result = _messageHandler.IdentifyRequest(testdata);
            result.ShouldBe(expected);

        }

        

    }
}
