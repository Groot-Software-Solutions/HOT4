using FluentAssertions;
using Hot.Application.Common.Interfaces;
using Hot.Tests.Common.Fakers.DbContext.Tables;
using Hot.Domain.Entities;
using Hot.Tests.Common.Fakers;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Vouchers;

namespace Hot.Infrastructure.Tests.RechargeServices
{
    public class PinRechargeServiceTests
    {

        [Fact]
        public async Task PinRechargeRechargeService_ShouldReturnRechargeResult_Test()
        {
            //Arrange
            var listPins = PinDbFacker.GetPinList(10);
            var service = Substitute.For<IDbContext>();
            service.Pins.RechargeAsync(Arg.Any<decimal>(), Arg.Any<int>(), Arg.Any<long>()).Returns(listPins);

            TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);

            var sut = new PinRechargeHandler(dbcontext, serviceProvider);
            //Act
            var result = await sut.ProcessAsync(recharge, rechargePrepaid);

            //Assert
            result.IsT0.Should().BeTrue(); 
            var rechargeResult = result.AsT0;
            rechargeResult.Successful.Should().Be(true);
            rechargeResult.Recharge.Should().Be(recharge);
            rechargeResult.Data.Should().Be(listPins);


        }

    }
}
