using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Application.Actions.LimitActions;
using Hot.Application.Common.Exceptions;

namespace Hot.Application.Tests.Actions
{
    public class LimitActionsTests
    {
        [Fact]   
        public async Task GetAccountLimit_ShouldReturnCurrentAccountLimit()


        {   var limit = LimitDbFacker.GetLimit();
            var logger = LoggerFaker.GetLogger<GetLimitByAccountIdQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().AccountLimit(limit);
            var request = new AccountLimitQuery(123455);
            var sut = new GetLimitByAccountIdQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.Should().NotBeNull();
            result.AsT0.DailyLimit.Should().Be(12);
            await _dbcontext.Limits.Received().GetCurrentLimitsAsync(Arg.Any<int>(), Arg.Any<long>());

        }
        
       
        [Fact]
        public async Task AddLimitCommand_ShouldBeSuccesful_Test()
        {
            //Arrange
            var limit = LimitDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<AddLimitCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().LimitAddSuccessful();
            var request = new AddLimitCommand(limit);
            var sut = new AddLimitCommandHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(1);
            await _dbcontext.Limits.Received(1).AddAsync(limit);
           
        }


        [Fact]
        public async Task AddLimitCommand_ShouldBeFailed_Test()
        {
            //Arrange
            var limit = LimitDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<AddLimitCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().LimitAddFailed();
            var request = new AddLimitCommand(limit);
            var sut = new AddLimitCommandHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT1.Should().BeTrue();
            result.AsT1.Should().BeOfType<AppException>();
            result.AsT1.Message.ToLower().Should().Contain("error");
            _dbcontext.Received(1);
            logger.Received(2);
        }
    }
}
