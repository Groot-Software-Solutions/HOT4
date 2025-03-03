using Hot.Application.Actions.RechargeActions;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Models;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.Services;
using Hot.Domain.Entities;
using System;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using AutoMapper;
using MediatR;

namespace Hot.Application.Tests.Actions
{
    public class RechargeActionsTests
    {
        private readonly IMapper mapper = Substitute.For<IMapper>();
        private readonly IMediator mediator = Substitute.For<IMediator>();

        [Fact]
        public async Task SearchRechargeByDates_ShouldReturnList_Test()
        {
            //Arrang
            var logger = LoggerFaker.GetLogger<SearchRechargesByAccountCommandHandler>();
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomRechargeSelectAgrregatorList(10);
            var request = new SearchRechargesByDates(1236, DateTime.Now, DateTime.Now);
            var sut = new SearchRechargesByAccountCommandHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());

            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().HaveCount(10);
            await _dbcontext.Received(2).Recharges.SelectAggregatorAsync(Arg.Any<long>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());

        }
        [Fact]
        public async Task SearchRechargeByAccount_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().RandomRechargesFindByAccount(10);
            var logger = LoggerFaker.GetLogger<SearchRechargeByAccountQueryHandler>();
            var request = new SearchRechargeByAccountQuery(12345, "0773404368");
            var sut = new SearchRechargeByAccountQueryHandler(_dbcontext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Should().HaveCount(10);
            await _dbcontext.Received().Recharges.FindByAccountAsync(Arg.Any<long>(), Arg.Any<string>());
        }
        [Fact]
        public async Task SearchRechargesByMobile_ShouldReturnList_Test()
        {
            //Arrange
            var _dbcontext = DbContextFaker.Mock()
                .Gives().FindByMobile(10);
            var logger = LoggerFaker.GetLogger<SearchRechargeByMobileQueryHandler>();
            var recharges = RechargeDbFacker.GetList(10);
            var request = new SearchRechargeByMobileQuery("0773404368");
            var sut = new SearchRechargeByMobileQueryHandler(_dbcontext, logger, mapper);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            var list = result.AsT0;
            list.Should().NotBeNull();
            list.Count.Should().Be(10);
            await _dbcontext.Received().Recharges.FindByMobileAsync(Arg.Is<string>(a => a == request.Mobile));
        }
        [Fact]
        public async Task ProcessRechargeAirtime_ShouldReturnRechargeResult_Test()
        {
            //Arrange
            var access = AccessDbFacker.GetSingle();
            var brand = BrandDbFacker.GetSingle();
            brand.BrandId = 1;
            brand.NetworkId = 2;
            brand.WalletTypeId = 1;
            var account = AccountDbFacker.GetSingle();
            var logger = LoggerFaker.GetLogger<ProcessRechargeCommandHandler>();
            var serviceFactory = Substitute.For<IRechargeHandlerFactory>();

            var _dbcontext = DbContextFaker.Mock()
                .Gives().Access(access)
                .Gives().Account(account)
                .Gives().BrandIdentify(brand.BrandId)
                .Gives().Brand(brand)
                .Gives().RandomProfileDiscount()
                .Gives().Network(brand.NetworkId)
                .Gives().RechargeAdd()
                .Gives().RechargePrepaidAdd();

            var rechargeResult = new RechargeResult() { Successful = true, Message = "Recharge Successful" };
            var rechargeService = Substitute.For<IRechargeHandler>();
            rechargeService.ProcessAsync(Arg.Any<Recharge>(), Arg.Any<RechargePrepaid>()).Returns(rechargeResult);
            serviceFactory.HasService(brand.BrandId).Returns(true);
            serviceFactory.GetService(brand.BrandId).Returns(rechargeService);
            // serviceFactory.GetWalletBalance(Arg.Any<string>(),Arg.Any<Account>()).Returns(789.23);
            var request = new ProcessRechargeAirtimeCommand(Domain.Enums.Brands.EconetUSD, "0773404368", 123, 123456);
            var sut = new ProcessRechargeCommandHandler(serviceFactory, _dbcontext, logger, mediator);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            //Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Successful.Should().Be(true);
            await _dbcontext.Received().Accesss.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Accounts.GetAsync(Arg.Any<int>());
            await _dbcontext.Received().Brands.IndentifyAsync(Arg.Any<int>(), Arg.Any<string>());
            await _dbcontext.Received().Networks.IndentifyAsync(Arg.Any<string>());
            await _dbcontext.Received().ProfileDiscounts.DiscountAsync(Arg.Any<int>(), Arg.Any<int>());
            await rechargeService.Received().ProcessAsync(Arg.Any<Recharge>(), Arg.Any<RechargePrepaid>());
            serviceFactory.Received().HasService(brand.BrandId);
            serviceFactory.Received().GetService(brand.BrandId);
        }
    }
}

