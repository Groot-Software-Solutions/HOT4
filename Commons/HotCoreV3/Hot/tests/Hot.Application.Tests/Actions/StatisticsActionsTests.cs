using Hot.Application.Actions.StatisticActions;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;

namespace Hot.Application.Tests.Actions
{
    public class StatisticsActionsTests
    {
        [Fact]
        public async Task GetRechargeLastDayTraffic_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomRechargeTrafficLastDay(5);
            var logger = LoggerFaker.GetLogger<GetRechargeLastDayTrafficQueryHandler>();
            var request = new GetRechargeLastDayTrafficQuery();
            var sut = new GetRechargeLastDayTrafficQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().NotBeNull();
            var list = result.AsT0;  
            list.Should().NotBeNull(); 
            list.Count.Should().Be(1);
            await _dbcontext.Received().Statistics.GetRechargeTrafficLastDayAsync();
        }
        [Fact]
        public async Task GetRechargeLatestTraffic_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomRechargeLatestTraffic(5);
            var logger = LoggerFaker.GetLogger<GetRechargeLastestDayTrafficQueryHandler>();
            var request = new GetRechargeLatestTrafficQuery();
            var sut = new GetRechargeLastestDayTrafficQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().NotBeNull();
            result.AsT0.Count.Should().Be(1);
            await _dbcontext.Received().Statistics.GetRechargeTrafficAsync();
        }
        [Fact]
        public async Task GetSMSTraffic_ShouldReturnlist_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomSMSTraffic(5);
            var logger = LoggerFaker.GetLogger<GetSmsTrafficQueryHandler>();
            var request = new GetSmsTrafficQuery();
            var sut = new GetSmsTrafficQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().NotBeNull();
            result.AsT0.Count.Should().Be(1);
            await _dbcontext.Received().Statistics.GetSmsTrafficAsync();
        }       
    }
}
