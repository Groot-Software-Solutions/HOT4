using Hot.Application.Actions.PinActions;
using Hot.Tests.Common.Fakers.DbContext;
using MediatR;
using Hot.Tests.Common.Fakers.Services;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using System.Data;
using System;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;

namespace Hot.Application.Tests.Actions
{
    public class PinActionsTests
    {
        private readonly IDbHelper dbhelper =
    Substitute.For<IDbHelper>();
        private readonly IMediator mediator =
        Substitute.For<IMediator>();

        [Fact]
        public async Task AvailablePinStock_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<AvailablePinStockQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPinStockList(12);
            var request = new AvailablePinStockQuery();
            var sut = new AvailablePinStockQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(12);
            await _dbcontext.Received().Pins.StockAsync();
        }

        [Fact]
        public async Task AvailablePromotionPinStock_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<AvailablePromotionPinsQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPromoStockList(10);
            var request = new AvailablePromotionPinStockQuery();
            var sut = new AvailablePromotionPinsQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().Pins.PromoStockAsync();
        }
        [Fact]
        public async Task getPinStock_ShouldReturnList_Test()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetPinStockQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPinStockList(10);
            var request = new GetPinStockQuery();
            var sut = new GetPinStockQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().Pins.StockAsync();
        }
        [Fact]
        public async Task PurchasePromoPin_ShouldReturnList()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomPromoRechargesList(10);
            var logger = LoggerFaker.GetLogger<PurchasePromoPinCommandHandler>();
            var request = new PurchasePromoPinCommand("0773404368", 2, 123, "0773263564");
            var sut = new PurchasePromoPinCommandHandler(_dbcontext, logger, mediator);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            //await _dbcontext.Received().Pins.PromoRechargeAsync(Arg.Is<string>(a => a == request.AccessCode), Arg.Is<int>(a => a == request.BrandId), Arg.Is<decimal>(a => a == request.Value), Arg.Is<string>(a => a == "0773263564"));
        }
        [Fact]
        public async Task AccountHasPurchasedPromotionPin_ShouldReturnBool()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<AccountHasPurchasedPromoPinQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().PromoHasPurchased(true);
            var request = new AccountHasPurchasedPromotionPinQuery(123456);
            var sut = new AccountHasPurchasedPromoPinQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Should().Be(true);
            //await _dbcontext.Received().Pins.PromoHasPurchased(Arg.Any<long>());
        }

        [Fact]
        public async Task GetPinsByBatchId_ShouldReturnList()
        {
            //Arrange
            var logger = LoggerFaker.GetLogger<GetPinsLoadedByBatchIdQueryHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomStockLoadedInBatch(10);
            var request = new GetPinsLoadedByBatchIdQuery(123);
            var sut = new GetPinsLoadedByBatchIdQueryHandler(_dbcontext, logger);

            //Act
            var result = await sut.Handle(request, new CancellationToken());


            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);

        }

        [Fact]
        public async Task SavePin_ShouldReturnInt()
        {
            //Arrange 
     
            var dbconn = Substitute.For<IDbConnection>();
            var dbtran = Substitute.For<IDbTransaction>();
            dbhelper.BeginTransaction().Returns(new Tuple<IDbConnection, IDbTransaction>(dbconn, dbtran));
            dbhelper.CommitTransaction(Arg.Any<IDbTransaction>()).Returns(true);
            var pinData = PinDbFacker.GetPinFileData();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().SavePin(10);
            var request = new SavePinCommand(pinData);
            var sut = new SavePinCommandHandler(_dbcontext, dbhelper);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
         
            await _dbcontext.Received().PinBatches.AddAsync(Arg.Any<PinBatch>());
        }


    }
}
