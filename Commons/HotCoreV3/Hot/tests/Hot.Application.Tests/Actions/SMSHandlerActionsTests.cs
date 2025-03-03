using Hot.Application.Actions;
using Hot.Application.Common.Interfaces;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Domain.Enums;
using MediatR;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;

namespace Hot.Application.Tests.Actions
{
    public class SMSHandlerActionsTests
    {
        private readonly IMediator mediator =
     Substitute.For<IMediator>();
        private readonly IHotTypeIdentifier identifier = 
            Substitute.For<IHotTypeIdentifier>();
        private readonly IMessageHandlerFactory messageHandlerFactory =
            Substitute.For<IMessageHandlerFactory>();
        [Fact]
        public async Task HandlePendingSMS_ShouldReturnUnit_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().SMSInbox(10);
            var logger = LoggerFaker.GetLogger<CheckPendingSMSCommandHandler>();
            var request = new HandlePendingSMSCommand();
            var sut = new CheckPendingSMSCommandHandler(_dbcontext, logger, mediator);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            await _dbcontext.Received().SMSs.InboxAsync();
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task HandleSMSCommand_ShouldReturnBoolean_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<HandleSMSCommandHandler>();
            var sms = SMSDbFacker.GetSingle();
            var request = new HandleSMSCommand(sms);
            HotTypes type = identifier.Identify(request.Sms.SMSText);
            ISMSMessageHandler handler = messageHandlerFactory.GetHandlerByType(type);
            await handler.HandleSMSAsync(request.Sms);            
            var sut = new HandleSMSCommandHandler(logger, identifier, messageHandlerFactory);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.Should().BeTrue();
        }      
    }
}
