using Bogus;
using Hot.Application.Common.Interfaces;
using Hot.Application.Common.Interfaces.RechargeServices;
using Hot.Tests.Common.Fakers;
using Hot.Domain.Entities;
using Hot.Tests.Common.Assertions;
using Hot.Application.Common.Models.RechargeServiceModels.Econet;
using Hot.Infrastructure.RechargeServices.RechargeHandlers.Airtime;

namespace Hot.Infrastructure.Tests.RechargeServices
{
    public class EconetRechargeServiceTests
    {

        [Theory]
        [MemberData(nameof(RechargeTests))]
        public async Task Recharge_Test(EconetRechargeResult serviceResult, bool expected, byte resultState)
        {
            //Arrange    
            var service = RechargeAPIService(serviceResult);
            TestEnvironment.GetRechargeTestItems(service, out IServiceProvider serviceProvider, out IDbContext dbcontext, out Recharge recharge, out RechargePrepaid rechargePrepaid);
            var sut = new EconetRechargeHandler(dbcontext, serviceProvider);
            //Act
            var result = await sut.ProcessAsync(recharge, rechargePrepaid);
            //Assert
            await RechargeAssertions.AssertRechargeResult(expected, resultState, dbcontext, recharge, result, serviceResult.RawResponseData ?? "");
            await service.Received().Recharge(Arg.Is<string>(m => m == recharge.Mobile), Arg.Is<decimal>(amt => amt == recharge.Amount), Arg.Any<string>());
        }


        private static IEconetRechargeAPIService RechargeAPIService(EconetRechargeResult RechargeResult)
        {
            var service = Substitute.For<IEconetRechargeAPIService>();
            service.Recharge(Arg.Any<string>(), Arg.Any<decimal>(), Arg.Any<string>()).Returns(RechargeResult);
            return service;
        }
        public static IEnumerable<object[]> RechargeTests()
        {
            return new List<object[]>
        {
            new object[]{ SuccessulfulResult(),true,2},
            new object[]{ FailedResult(),false,3}
        };
        }

        private static EconetRechargeResult SuccessulfulResult()
        {
            var result = RandomResult();
            result.Successful = true;
            result.RawResponseData = "Success";
            return result;
        }
        private static EconetRechargeResult FailedResult()
        {
            var result = RandomResult();
            result.Successful = false;
            return result;
        }
        private static EconetRechargeResult RandomResult()
        {
            var econetRechargeResult = new Faker<EconetRechargeResult>()
              .RuleFor(a => a.Successful, true)
              .RuleFor(a => a.RawResponseData, f => f.Random.String())
              .RuleFor(a => a.TransactionResult, f => f.Random.String());
            return econetRechargeResult.Generate();
        }


    }
}
