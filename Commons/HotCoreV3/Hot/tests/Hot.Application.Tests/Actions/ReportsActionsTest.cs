using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Application.Actions.ReportsActions;
using System;
using AutoMapper;
using Hot.Application.Common.Models;

namespace Hot.Application.Tests.Actions
{
    public class ReportsActionsTests
    {
        private readonly IMapper mapper =
        Substitute.For<IMapper>();


        [Fact]
        public async Task GetEconetStatsTest_ShouldReturnList()
        {
            //Arrange
            DateTime startDate = Convert.ToDateTime("01-01-2007");
            DateTime endDate = DateTime.Now;
            var logger = LoggerFaker.GetLogger<GetEconetStatsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomEconetStatsList(4);
            var request = new GetEconetStatsQuery(startDate,endDate );
            var sut = new GetEconetStatsQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(4);
            await _dbcontext.Received().Report.GetEconetStatsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());


        } 
        [Fact]
        public async Task GetPaymentsTest_ShouldReturnList()
        {
            //Arrange
            DateTime startDate = Convert.ToDateTime("01-01-2018");
            DateTime endDate = DateTime.Now;
            int reportTypeId = 123433;
            long accountId = 12345678113;
            int paymentTypeId = 12334;
            int bankId = 12889;
            var logger = LoggerFaker.GetLogger<GetPaymentsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPaymentList(4);
            var request = new GetPaymentsQuery(startDate,endDate, reportTypeId,accountId,paymentTypeId,bankId );
            var sut = new GetPaymentsQueryHandler(_dbcontext, logger);
 
            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(4);
            await _dbcontext.Received().Report.GetPaymentsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>());


        }

        [Fact]
        public async Task GetProfileDiscountsTest_ShouldReturnList()
        {
            //Arrange
            int  reportId = 12342;
            int walletId = 123456781;
            var logger = LoggerFaker.GetLogger<GetProfileDiscountsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomProfileDiscountList(6);
            var request = new GetProfileDiscountsQuery( reportId,walletId);
            var sut = new GetProfileDiscountsQueryHandler(_dbcontext, logger);
 
            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(6);
            await _dbcontext.Received().Report.GetProfileDiscountsAsync(Arg.Any<int>(), Arg.Any<int>());


        }

        [Fact]
        public async Task GetPeriodicTest_ShouldReturnList()
        {
            //Arrange
            DateTime startDate = Convert.ToDateTime("01-01-2006");
            DateTime endDate = DateTime.Now;
            int reportTypeId = 1234456;
            long networkId = 123456789;
            int accountId = 12345;

            var logger = LoggerFaker.GetLogger<GetStatsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPeriodicStatsList(5);
            var request = new GetStatsQuery(startDate, endDate, reportTypeId, networkId, accountId);
            var sut = new GetStatsQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(5);
           


        }

        [Fact]
        public async Task GetRunningBalance_ShouldReturnList()
        {
            //Arrange

            DateTime startDate = Convert.ToDateTime("01-01-2006");
            DateTime endDate = DateTime.Now;
            long accountId = 12345;
            //var _mapper = new MapperConfiguration(opt => opt.AddProfile(new PinBatchDetailedModelProfile())).CreateMapper();
            StatementTransactionModel TransactionModel = new();
            mapper.Map<StatementTransactionModel>(Arg.Any<StatementTransaction>()).Returns(TransactionModel);
            var logger = LoggerFaker.GetLogger<GetRunningBalanceForClientQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomStatement(5)
                .Gives().StartingBalance(2);


            var request = new GetRunningBalanceForClientQuery(accountId, startDate, endDate);
            var sut = new GetRunningBalanceForClientQueryHandler(_dbcontext, logger, mapper);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            var list = result.AsT0; 
            list.Should().NotBeNull();
            list.Should().HaveCount(5);

            

        }


        [Fact]
        public async Task GetTransactions_ShouldReturnList2()
        {
            //Arrange
            DateTime startDate = Convert.ToDateTime("01-01-2020");
            DateTime endDate = DateTime.Now;
            int reportTypeId = 1234;
            long accountId = 12345678;
            int walletId = 123;
            int bankId = 43231;

            var logger = LoggerFaker.GetLogger<GetTransactionsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomStatementTransactionList(10);
            var request = new GetTransactionsQuery(startDate, endDate, reportTypeId, accountId, walletId, bankId);
            var sut = new GetTransactionsQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            //await _dbcontext.Received().Report.GetPaymentsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<long>(), Arg.Any<int>(), Arg.Any<int>());


        }




    }
}
