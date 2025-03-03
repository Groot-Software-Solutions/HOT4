using FluentAssertions;
using Hot.Application.Actions.RechargeActions;
using Hot.Application.Common.Interfaces;
using Hot.Domain.Entities;
using Hot.Nyaradzo.Application.Actions;
using Hot.Tests.Common.Assertions;
using Hot.Tests.Common.Fakers;
using Hot.Tests.Common.Fakers.DbContext;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Tests.Common.Fakers.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Hot.Nyaradzo.Test.Actions
{
    public class ProcessNyaradzoPaymentTests
    {
        [Fact]
        public async Task ProcessNyaradzoPayment_ShouldReturnNyaradzoResult_Test()
        {
            //Arrange
            var rechargeResult = DbContextFaker.GenerateRechargeResultTestData();
            var service = Substitute.For<IRechargeService>();
            service.ProcessAsync(Arg.Any<Recharge>(), Arg.Any<RechargePrepaid>()).Returns(rechargeResult);

            var serviceFactory = Substitute.For<IRechargeServiceFactory>();
            serviceFactory.HasService(Arg.Any<int>()).Returns(true);
            serviceFactory.GetRechargeService(Arg.Any<int>()).Returns(service);

            TestEnvironment.GetRechargeTestItems(service, out var serviceProvider, out var dbContext, out var recharge, out var rechargePrepaid);
            
            var brand = DbContextFaker.GenerateBrandTestData();
           
            dbContext = dbContext
               .Gives().GetBrand(brand)
               .Gives().RandomProfileDiscount(); 

            var logger = LoggerFaker.GetLogger<ProcessRechargeCommandHandler>();
           
            var request = new ProcessNyaradzoPayment(123456, "12345632", 500, 4, "0773404368");
            var sut = new ProcessNyaradzoPaymentHandler( serviceFactory,dbContext, logger);
            //Act
            var result = await sut.Handle(request, new CancellationToken());
            // Assert
            result.IsT0.Should().BeTrue();
            result.AsT0.Successful.Should().Be(true);
            serviceFactory.Received().HasService(Arg.Any<int>());
            serviceFactory.Received().GetRechargeService(Arg.Any<int>());
            await dbContext.Received().Accesss.GetAsync(Arg.Any<int>());
            await dbContext.Received().Accounts.GetAsync(Arg.Any<int>());
            await dbContext.Received().Brands.GetAsync(Arg.Any<int>());
            await dbContext.Received().ProfileDiscounts.DiscountAsync(Arg.Any<int>(), Arg.Any<int>());
            await service.Received().ProcessAsync(Arg.Any<Recharge>(), Arg.Any<RechargePrepaid>());
            //await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData);
        }
    }
}
