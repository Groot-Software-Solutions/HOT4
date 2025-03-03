using Hot.Application.Actions.SMSActions;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Domain.Entities;
using System;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;

namespace Hot.Application.Tests.Actions
{
    public class SMSActionsTests
    {
        [Fact]
        public async Task SearchByDates_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomSMSsSearchByDatesList(10);
            var logger = LoggerFaker.GetLogger<SearchByDatesQueryHandler>();
            var request = new SearchByDatesQuery(DateTime.Now, DateTime.Now);
            var sut = new SearchByDatesQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Count.Should().Be(10);
            await _dbcontext.Received().SMSs.SearchByDatesAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
        }
        [Fact]
        public async Task SearchByFilter_SHouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomSMSSearchByFilterList(10);
            var logger = LoggerFaker.GetLogger<SearchSMSsByFilterQueryHandler>();
            var request = new SearchSMSsByFilterQuery("filter");
            var sut = new SearchSMSsByFilterQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().SMSs.SearchByFilterAsync(Arg.Any<string>());
        }
        [Fact]
        public async Task SearchByMobile_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomSMSSearchByMobileList(10);
            var logger = LoggerFaker.GetLogger<GetSMSsByMobilQueryHandler>();
            var request = new SearchSMSsByMobileQuery("0773404368");
            var sut = new GetSMSsByMobilQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().SMSs.SearchByMobileAsync(Arg.Any<string>());
        }
        [Fact]
        public async Task SendConfirmationSMS_ShouldReturnBool_Test()
        {
            //Arrange
            var template = TemplateDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Template(template);
            var logger = LoggerFaker.GetLogger<SendConfirmationSMStoDealerCommandHandler>();
            var request = new SendConfirmationSMStoDealerCommand(123,120,100,12,"0774123456","0778458456");
            var sut = new SendConfirmationSMStoDealerCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().Templates.GetAsync(Arg.Any<int>());
        }
        [Fact]
        public async Task SendPinToDealer_ShouldReturnBool_Test()
        {
            //Arrange
            var pin = PinDbFacker.GetSingle();
            var template = TemplateDbFacker.GetSingle();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().Template(template)
                .Gives().RandomBrandsList(10)
                .Gives().SMSAdd(234);
            var logger = LoggerFaker.GetLogger<SendPinToDealerSMSCommandHandler>();
            var request = new SendPinToDealerSMSCommand(pin, "0773404368");
            var sut = new SendPinToDealerSMSCommandHandler(_dbcontext, logger);
            //Act
            var result =await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            await _dbcontext.Received().Templates.GetAsync(Arg.Any<int>()); ;
            await _dbcontext.Received().Brands.ListAsync();
            await _dbcontext.Received().SMSs.AddAsync(Arg.Any<SMS>()); ;
        }      
    }
}
